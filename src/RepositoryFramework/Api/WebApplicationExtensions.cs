using Microsoft.AspNetCore.Builder;
using RepositoryFramework;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApplicationExtensions
    {
        internal static Dictionary<Type, RepositoryFrameworkService> Services = new();
        public static WebApplication AddApiForRepository<T>(this WebApplication app, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
            => app.AddApiForRepository(typeof(T), startingPath, authorizationPolicy);
        public static WebApplication AddApiForRepository(this WebApplication app, Type modelType, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
        {
            if (!Services.ContainsKey(modelType))
                throw new ArgumentException($"Please check if your {modelType.Name} model has a service injected for IRepository, IQuery, ICommand.");
            var serviceValue = Services[modelType];
            if (serviceValue.QueryType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(WebApplicationExtensions).GetMethod(nameof(AddGet), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.QueryType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(WebApplicationExtensions).GetMethod(nameof(AddQuery), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(WebApplicationExtensions).GetMethod(nameof(AddInsert), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(WebApplicationExtensions).GetMethod(nameof(AddUpdate), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(WebApplicationExtensions).GetMethod(nameof(AddDelete), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            return app;
        }
        public static WebApplication AddApiForRepositoryFramework(this WebApplication app, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
        {
            foreach (var service in Services)
                _ = app.AddApiForRepository(service.Key, startingPath, authorizationPolicy);
            return app;
        }
        private static RouteHandlerBuilder AddAuthorization(this RouteHandlerBuilder router, AuthorizationForApi authorization, AuthorizationPath path)
        {
            if (authorization != null && authorization.Path.HasFlag(path))
            {
                if (authorization.Policies != null)
                    router.RequireAuthorization(authorization.Policies);
                else
                    router.RequireAuthorization();
            }
            return router;
        }
        private static void AddGet<T, TKey, TPattern>(WebApplication app, string name, string startingPath, AuthorizationForApi authorization)
            where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name.ToLower()}/get", async (TKey key, TPattern pattern) =>
               {
                   var queryPattern = pattern as IQuery<T, TKey>;
                   return await queryPattern!.GetAsync(key);
               }).WithName($"Get{name}")
               .AddAuthorization(authorization, AuthorizationPath.Get);
        }
        private static void AddQuery<T, TKey, TPattern>(WebApplication app, string name, string startingPath, AuthorizationForApi authorization)
           where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name.ToLower()}/list", async (string? query, int? top, int? skip, TPattern pattern) =>
              {
                  dynamic? expression = null;
                  if (!string.IsNullOrWhiteSpace(query))
                  {
                      var parameter = Expression.Parameter(typeof(T), query.Split(' ').First());
                      expression = DynamicExpressionParser.ParseLambda<T, bool>(ParsingConfig.Default, false, query);
                  }
                  var queryPattern = pattern as IQuery<T, TKey>;
                  return await queryPattern!.QueryAsync(expression, top, skip);
              }).WithName($"List{name}")
              .AddAuthorization(authorization, AuthorizationPath.Query);
        }
        private static void AddInsert<T, TKey, TPattern>(WebApplication app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name.ToLower()}/insert", async (TKey key, T entity, TPattern pattern) =>
            {
                var commandPattern = pattern as ICommand<T, TKey>;
                return await commandPattern!.InsertAsync(key, entity);
            }).WithName($"Insert{name}")
            .AddAuthorization(authorization, AuthorizationPath.Insert);
        }
        private static void AddUpdate<T, TKey, TPattern>(WebApplication app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name.ToLower()}/update", async (TKey key, T entity, TPattern pattern) =>
            {
                var commandPattern = pattern as ICommand<T, TKey>;
                return await commandPattern!.UpdateAsync(key, entity);
            }).WithName($"Update{name}")
            .AddAuthorization(authorization, AuthorizationPath.Update);
        }
        private static void AddDelete<T, TKey, TPattern>(WebApplication app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name.ToLower()}/delete", async (TKey key, TPattern pattern) =>
            {
                var commandPattern = pattern as ICommand<T, TKey>;
                return await commandPattern!.DeleteAsync(key);
            }).WithName($"Delete{name}")
            .AddAuthorization(authorization, AuthorizationPath.Delete);
        }
    }
}
