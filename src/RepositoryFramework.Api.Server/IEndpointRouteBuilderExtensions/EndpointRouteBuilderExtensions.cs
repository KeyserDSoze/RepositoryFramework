using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Add repository or CQRS service injected as api for your <typeparamref name="T"/> model.
        /// </summary>
        /// <typeparam name="T">Model of your repository or CQRS that you want to add as api</typeparam>
        /// <param name="app">IEndpointRouteBuilder</param>
        /// <param name="startingPath">By default is "api", but you can choose your path. https://{your domain}/{startingPath}</param>
        /// <param name="authorizationPolicy">Add authorization to your api.</param>
        /// <returns>IEndpointRouteBuilder</returns>
        public static IEndpointRouteBuilder AddApiForRepository<T>(this IEndpointRouteBuilder app, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
            => app.AddApiForRepository(typeof(T), startingPath, authorizationPolicy);
        /// <summary>
        /// Add repository or CQRS service injected as api for your <paramref name="modelType"/> model.
        /// </summary>
        /// <typeparam name="TEndpointRouteBuilder"></typeparam>
        /// <param name="app">IEndpointRouteBuilder</param>
        /// <param name="modelType">type of your repository or CQRS model that you want to add as api</param>
        /// <param name="startingPath">By default is "api", but you can choose your path. https://{your domain}/{startingPath}</param>
        /// <param name="authorizationPolicy">Add authorization to your api.</param>
        /// <returns>IEndpointRouteBuilder</returns>
        public static TEndpointRouteBuilder AddApiForRepository<TEndpointRouteBuilder>(this TEndpointRouteBuilder app, Type modelType, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        {
            var registry = app.ServiceProvider.GetService<RepositoryFrameworkRegistry>();
            var serviceValue = registry!.Services.FirstOrDefault(x => x.ModelType == modelType);
            if (serviceValue == null)
                throw new ArgumentException($"Please check if your {modelType.Name} model has a service injected for IRepository, IQuery, ICommand.");
            if (serviceValue.QueryType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddGet), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, serviceValue.StateType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddQuery), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, serviceValue.StateType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddExist), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, serviceValue.StateType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddInsert), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, serviceValue.StateType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddUpdate), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, serviceValue.StateType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddDelete), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, serviceValue.StateType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorizationPolicy! });
            }
            return app;
        }
        /// <summary>
        /// Add all repository or CQRS services injected as api.
        /// </summary>
        /// <typeparam name="TEndpointRouteBuilder"></typeparam>
        /// <param name="app">IEndpointRouteBuilder</param>
        /// <param name="startingPath">By default is "api", but you can choose your path. https://{your domain}/{startingPath}</param>
        /// <param name="authorizationPolicy">Add authorization to all of your api.</param>
        /// <returns>IEndpointRouteBuilder</returns>
        public static TEndpointRouteBuilder AddApiForRepositoryFramework<TEndpointRouteBuilder>(this TEndpointRouteBuilder app, string startingPath = "api", AuthorizationForApi? authorizationPolicy = null)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        {
            var services = app.ServiceProvider.GetService<RepositoryFrameworkRegistry>();
            foreach (var service in services!.Services.Where(x => !x.IsPrivate))
                _ = app.AddApiForRepository(service.ModelType, startingPath, authorizationPolicy);
            return app;
        }
        private static RouteHandlerBuilder AddAuthorization(this RouteHandlerBuilder router, AuthorizationForApi authorization, RepositoryMethod path)
        {
            if (authorization != null &&
                (authorization.Path.HasFlag(RepositoryMethod.All) || authorization.Path.HasFlag(path)))
            {
                if (authorization.Policies != null)
                    router.RequireAuthorization(authorization.Policies);
                else
                    router.RequireAuthorization();
            }
            return router;
        }
        private static void AddGet<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
            where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Get)}", async (TKey key, [FromServices] TService service) =>
               {
                   var queryService = service as IQueryPattern<T, TKey, TState>;
                   return await queryService!.GetAsync(key);
               }).WithName($"{nameof(RepositoryMethod.Get)}{name}")
               .AddAuthorization(authorization, RepositoryMethod.Get);
        }
        private static void AddQuery<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
           where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Query)}", async (string? query, int? top, int? skip, [FromServices] TService service) =>
              {
                  dynamic? expression = null;
                  if (!string.IsNullOrWhiteSpace(query))
                      expression = DynamicExpressionParser.ParseLambda<T, bool>(ParsingConfig.Default, false, query);
                  var queryService = service as IQueryPattern<T, TKey, TState>;
                  return await queryService!.QueryAsync(expression, top, skip);
              
              }).WithName($"{nameof(RepositoryMethod.Query)}{name}")
              .AddAuthorization(authorization, RepositoryMethod.Query);
        }
        private static void AddExist<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
            where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Exist)}", async (TKey key, [FromServices] TService service) =>
            {
                var queryService = service as IQueryPattern<T, TKey, TState>;
                return await queryService!.ExistAsync(key);
            }).WithName($"{nameof(RepositoryMethod.Exist)}{name}")
               .AddAuthorization(authorization, RepositoryMethod.Exist);
        }
        private static void AddInsert<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethod.Insert)}", async (TKey key, T entity, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey, TState>;
                return await commandService!.InsertAsync(key, entity);
            }).WithName($"{nameof(RepositoryMethod.Insert)}{name}")
            .AddAuthorization(authorization, RepositoryMethod.Insert);
        }
        private static void AddUpdate<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethod.Update)}", async (TKey key, T entity, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey, TState>;
                return await commandService!.UpdateAsync(key, entity);
            }).WithName($"{nameof(RepositoryMethod.Update)}{name}")
            .AddAuthorization(authorization, RepositoryMethod.Update);
        }
        private static void AddDelete<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, AuthorizationForApi authorization)
          where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Delete)}", async (TKey key, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey, TState>;
                return await commandService!.DeleteAsync(key);
            }).WithName($"{nameof(RepositoryMethod.Delete)}{name}")
            .AddAuthorization(authorization, RepositoryMethod.Delete);
        }
    }
}
