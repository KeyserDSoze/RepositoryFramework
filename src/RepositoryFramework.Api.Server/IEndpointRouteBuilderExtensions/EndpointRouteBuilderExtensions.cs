using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Add repository or CQRS service injected as api for your <typeparamref name="T"/> model.
        /// </summary>
        /// <typeparam name="T">Model of your repository or CQRS that you want to add as api</typeparam>
        /// <param name="app">IEndpointRouteBuilder</param>
        /// <param name="startingPath">By default is "api", but you can choose your path. https://{your domain}/{startingPath}</param>
        /// <returns>ApiAuthorizationBuilder</returns>
        public static ApiAuthorizationBuilder AddApiForRepository<T>(this IEndpointRouteBuilder app, string startingPath = "api")
            => new(authorization => app.AddApiForRepository(typeof(T), startingPath, authorization));

        /// <summary>
        /// Add all repository or CQRS services injected as api.
        /// </summary>
        /// <typeparam name="TEndpointRouteBuilder"></typeparam>
        /// <param name="app">IEndpointRouteBuilder</param>
        /// <param name="startingPath">By default is "api", but you can choose your path. https://{your domain}/{startingPath}</param>
        /// <returns>ApiAuthorizationBuilder</returns>
        public static ApiAuthorizationBuilder AddApiForRepositoryFramework<TEndpointRouteBuilder>(this TEndpointRouteBuilder app,
            string startingPath = "api")
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        => new(authorization =>
                {
                    var services = app.ServiceProvider.GetService<RepositoryFrameworkRegistry>();
                    foreach (var service in services!.Services.Where(x => !x.NotExposableAsApi))
                        _ = app.AddApiForRepository(service.ModelType, startingPath, authorization);
                    return app;
                });

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "I need reflection in this point to allow the creation of T methods at runtime.")]
        private static TEndpointRouteBuilder AddApiForRepository<TEndpointRouteBuilder>(this TEndpointRouteBuilder app, Type modelType, string startingPath = "api", ApiAuthorization? authorization = null)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        {
            var registry = app.ServiceProvider.GetService<RepositoryFrameworkRegistry>();
            var serviceValue = registry!.Services.FirstOrDefault(x => x.ModelType == modelType);
            if (serviceValue == null)
                throw new ArgumentException($"Please check if your {modelType.Name} model has a service injected for IRepository, IQuery, ICommand.");
            if (serviceValue.QueryType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddGet), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddQuery), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddCount), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddExist), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.QueryType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });
            }
            if (serviceValue.CommandType != null || serviceValue.RepositoryType != null)
            {
                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddInsert), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddUpdate), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddDelete), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });

                _ = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(AddBatch), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(modelType, serviceValue.KeyType, (serviceValue.CommandType ?? serviceValue.RepositoryType)!)
                    .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });
            }
            return app;
        }
        private static void AddGet<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
           where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Get)}", async (string key, [FromServices] TService service) =>
            {
                var queryService = service as IQueryPattern<T, TKey>;
                var keyAsValue = parser(key);
                return await queryService!.GetAsync(keyAsValue).NoContext();
            }).WithName($"{nameof(RepositoryMethods.Get)}{name}")
               .AddAuthorization(authorization, RepositoryMethods.Get);
        }
        private static void AddQuery<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Query)}",
                async (string? query, int? top, int? skip, string? order, bool? asc, [FromServices] TService service) =>
                {
                    var options = QueryOptions<T>.ComposeFromQuery(query, top, skip, order, asc);
                    var queryService = service as IQueryPattern<T, TKey>;
                    return await queryService!.QueryAsync(options).NoContext();

                }).WithName($"{nameof(RepositoryMethods.Query)}{name}")
              .AddAuthorization(authorization, RepositoryMethods.Query);
        }
        private static void AddCount<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Count)}",
                async (string? query, int? top, int? skip, string? order, bool? asc, [FromServices] TService service) =>
                {
                    var options = QueryOptions<T>.ComposeFromQuery(query, top, skip, order, asc);
                    var queryService = service as IQueryPattern<T, TKey>;
                    return await queryService!.CountAsync(options).NoContext();

                }).WithName($"{nameof(RepositoryMethods.Count)}{name}")
              .AddAuthorization(authorization, RepositoryMethods.Count);
        }
        private static void AddExist<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Exist)}", async (string key, [FromServices] TService service) =>
            {
                var queryService = service as IQueryPattern<T, TKey>;
                var keyAsValue = parser(key);
                return await queryService!.ExistAsync(keyAsValue).NoContext();
            }).WithName($"{nameof(RepositoryMethods.Exist)}{name}")
               .AddAuthorization(authorization, RepositoryMethods.Exist);
        }
        private static void AddInsert<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Insert)}", async (string key, T entity, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey>;
                var keyAsValue = parser(key);
                return await commandService!.InsertAsync(keyAsValue, entity).NoContext();
            }).WithName($"{nameof(RepositoryMethods.Insert)}{name}")
            .AddAuthorization(authorization, RepositoryMethods.Insert);
        }
        private static void AddUpdate<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Update)}", async (string key, T entity, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey>;
                var keyAsValue = parser(key);
                return await commandService!.UpdateAsync(keyAsValue, entity).NoContext();
            }).WithName($"{nameof(RepositoryMethods.Update)}{name}")
            .AddAuthorization(authorization, RepositoryMethods.Update);
        }
        private static void AddBatch<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Batch)}", async (BatchOperations<T, TKey> operations, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey>;
                return await commandService!.BatchAsync(operations).NoContext();
            }).WithName($"{nameof(RepositoryMethods.Batch)}{name}")
            .AddAuthorization(authorization, RepositoryMethods.Batch);
        }
        private static void AddDelete<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Delete)}", async (string key, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey>;
                var keyAsValue = parser(key);
                return await commandService!.DeleteAsync(keyAsValue).NoContext();
            }).WithName($"{nameof(RepositoryMethods.Delete)}{name}")
                .AddAuthorization(authorization, RepositoryMethods.Delete);
        }
        private static Func<string, TKey> GetKeyParser<TKey>()
        {
            Type type = typeof(TKey);
            bool hasProperties = type.GetProperties().Length > 0;
            if (hasProperties)
                return key => key.FromJson<TKey>();
            if (type == typeof(Guid))
                return key => (dynamic)Guid.Parse(key);
            else if (type == typeof(DateTimeOffset))
                return key => (dynamic)DateTimeOffset.Parse(key);
            else if (type == typeof(TimeSpan))
                return key => (dynamic)TimeSpan.Parse(key);
            else if (type == typeof(nint))
                return key => (dynamic)nint.Parse(key);
            else if (type == typeof(nuint))
                return key => (dynamic)nuint.Parse(key);
            else
                return key => (TKey)Convert.ChangeType(key, type);
        }
        private static RouteHandlerBuilder AddAuthorization(this RouteHandlerBuilder router, ApiAuthorization? authorization, RepositoryMethods path)
        {
            var policies = authorization?.GetPolicy(path);
            if (policies != null)
                router.RequireAuthorization(policies);
            return router;
        }
    }
}