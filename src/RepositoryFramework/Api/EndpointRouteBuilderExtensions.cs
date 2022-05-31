using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder AddApiForRepository<T>(this IEndpointRouteBuilder app, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
            => app.AddApiForRepository(typeof(T), startingPath, authorizationPolicy);
        public static TEndpointRouteBuilder AddApiForRepository<TEndpointRouteBuilder>(this TEndpointRouteBuilder app, Type modelType, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        {
            var services = app.ServiceProvider.GetService<RepositoryFrameworkServices>();
            var serviceValue = services!.Services.FirstOrDefault(x => x.ModelType == modelType);
            if (serviceValue == null)
                throw new ArgumentException($"Please check if your {modelType.Name} model has a service injected for IRepository, IQuery, ICommand.");
            if (serviceValue.QueryType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddGet), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.QueryType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddQuery), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddInsert), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddUpdate), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddDelete), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            return app;
        }
        public static TEndpointRouteBuilder AddApiForRepositoryFramework<TEndpointRouteBuilder>(this TEndpointRouteBuilder app, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        {
            var services = app.ServiceProvider.GetService<RepositoryFrameworkServices>();
            foreach (var service in services!.Services)
                _ = app.AddApiForRepository(service.ModelType, startingPath, authorizationPolicy);
            return app;
        }
        private static RouteHandlerBuilder AddAuthorization(this RouteHandlerBuilder router, AuthorizationForApi authorization, ApiName path)
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
        private static void AddGet<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
            where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(ApiName.Get)}", async (TKey key, TService service) =>
               {
                   var queryService = service as IQuery<T, TKey>;
                   return await queryService!.GetAsync(key);
               }).WithName($"{nameof(ApiName.Get)}{name}")
               .AddAuthorization(authorization, ApiName.Get);
        }
        private static void AddQuery<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
           where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(ApiName.Search)}", async (string? query, int? top, int? skip, TService service) =>
              {
                  dynamic? expression = null;
                  if (!string.IsNullOrWhiteSpace(query))
                  {
                      var parameter = Expression.Parameter(typeof(T), query.Split(' ').First());
                      expression = DynamicExpressionParser.ParseLambda<T, bool>(ParsingConfig.Default, false, query);
                  }
                  var queryService = service as IQuery<T, TKey>;
                  return await queryService!.QueryAsync(expression, top, skip);
              }).WithName($"{nameof(ApiName.Search)}{name}")
              .AddAuthorization(authorization, ApiName.Search);
        }
        private static void AddInsert<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(ApiName.Insert)}", async (TKey key, T entity, TService service) =>
            {
                var commandService = service as ICommand<T, TKey>;
                return await commandService!.InsertAsync(key, entity);
            }).WithName($"{nameof(ApiName.Insert)}{name}")
            .AddAuthorization(authorization, ApiName.Insert);
        }
        private static void AddUpdate<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(ApiName.Update)}", async (TKey key, T entity, TService service) =>
            {
                var commandService = service as ICommand<T, TKey>;
                return await commandService!.UpdateAsync(key, entity);
            }).WithName($"{nameof(ApiName.Update)}{name}")
            .AddAuthorization(authorization, ApiName.Update);
        }
        private static void AddDelete<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(ApiName.Delete)}", async (TKey key, TService service) =>
            {
                var commandService = service as ICommand<T, TKey>;
                return await commandService!.DeleteAsync(key);
            }).WithName($"{nameof(ApiName.Delete)}{name}")
            .AddAuthorization(authorization, ApiName.Delete);
        }
    }
}
