﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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

        private const string NotImplementedExceptionIlOperation = "newobj instance void System.NotImplementedException";
        private static readonly List<string> PossibleMethods = new()
        {
            nameof(AddGet),
            nameof(AddQuery),
            nameof(AddExist),
            nameof(AddOperation),
            nameof(AddInsert),
            nameof(AddUpdate),
            nameof(AddDelete),
            nameof(AddBatch),
        };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "I need reflection in this point to allow the creation of T methods at runtime.")]
        private static TEndpointRouteBuilder AddApiForRepository<TEndpointRouteBuilder>(this TEndpointRouteBuilder app, Type modelType, string startingPath = "api", ApiAuthorization? authorization = null)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        {
            var registry = app.ServiceProvider.GetService<RepositoryFrameworkRegistry>();
            var serviceValue = registry!.Services.FirstOrDefault(x => x.ModelType == modelType);
            if (serviceValue == null)
                throw new ArgumentException($"Please check if your {modelType.Name} model has a service injected for IRepository, IQuery, ICommand.");

            Dictionary<string, bool> configuredMethods = new();

            foreach (var type in serviceValue.RepositoryTypes.Select(x => x.Value))
                foreach (var method in type.CurrentType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    var currentMethodName = PossibleMethods.FirstOrDefault(x => x == $"Add{method.Name.Replace("Async", string.Empty)}");
                    if (!string.IsNullOrWhiteSpace(currentMethodName) && !configuredMethods.ContainsKey(currentMethodName))
                    {
                        bool isNotImplemented = false;
                        Try.WithDefaultOnCatch(() =>
                        {
                            var instructions = method.GetBodyAsString();
                            isNotImplemented = instructions.Contains(NotImplementedExceptionIlOperation);
                        });
                        if (isNotImplemented)
                            continue;

                        _ = typeof(EndpointRouteBuilderExtensions).GetMethod(currentMethodName, BindingFlags.NonPublic | BindingFlags.Static)!
                           .MakeGenericMethod(modelType, serviceValue.KeyType, type.InterfaceType)
                           .Invoke(null, new object[] { app, modelType.Name, startingPath, authorization! });
                        configuredMethods.Add(currentMethodName, true);
                    }
                }

            return app;
        }
        private static void AddGet<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
           where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Get)}",
                async ([FromQuery] string key, [FromServices] TService service) =>
            {
                var queryService = service as IQueryPattern<T, TKey>;
                var keyAsValue = parser(key);
                return await queryService!.GetAsync(keyAsValue).NoContext();
            }).WithName($"{nameof(RepositoryMethods.Get)}{name}")
               .AddAuthorization(authorization, RepositoryMethods.Get);
        }
#warning think about to have more than one api with the same name and a dictionary? Problem with change of mind? the key name can be used?
        private static void AddQuery<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Query)}",
                async ([FromBody] SerializableQuery query, [FromServices] TService service) =>
                {
                    var options = query.DeserializeAndTranslate<T>();
                    var queryService = service as IQueryPattern<T, TKey>;
                    return await queryService!.QueryAsync(options).ToListAsync().NoContext();

                }).WithName($"{nameof(RepositoryMethods.Query)}{name}")
              .AddAuthorization(authorization, RepositoryMethods.Query);
        }
        private static void AddOperation<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Operation)}",
                async ([FromQuery] Operations op, [FromQuery] string? returnType,
                    [FromBody] SerializableQuery query, [FromServices] TService service) =>
                {
                    var options = query.DeserializeAndTranslate<T>();
                    Type type = CalculateTypeFromQuery();
                    var queryService = service as IQueryPattern<T, TKey>;
                    var result = await Generics.WithStatic(
                                  typeof(EndpointRouteBuilderExtensions),
                                  nameof(GetResultFromOperation),
                                  typeof(T), typeof(TKey), type)
                                .InvokeAsync(queryService!, op, options)!;
                    return result;

                    Type CalculateTypeFromQuery()
                    {
                        Type? calculatedType = typeof(object);
                        if (string.IsNullOrWhiteSpace(returnType))
                            return calculatedType;
                        if (PrimitiveMapper.Instance.FromNameToAssemblyQualifiedName.ContainsKey(returnType))
                            calculatedType = Type.GetType(PrimitiveMapper.Instance.FromNameToAssemblyQualifiedName[returnType]);
                        else
                            calculatedType = Type.GetType(returnType);
                        return calculatedType ?? typeof(object);
                    }
                }).WithName($"{nameof(RepositoryMethods.Operation)}{name}")
              .AddAuthorization(authorization, RepositoryMethods.Operation);
        }
        private static ValueTask<TProperty> GetResultFromOperation<T, TKey, TProperty>(
            IQueryPattern<T, TKey> queryService,
            Operations operations,
            Query options)
            where TKey : notnull
            => queryService.OperationAsync(
                new OperationType<TProperty> { Operation = operations },
                options);
        private static void AddExist<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Exist)}",
                async ([FromQuery] string key, [FromServices] TService service) =>
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
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Insert)}",
                async ([FromQuery] string key, [FromBody] T entity, [FromServices] TService service) =>
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
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Update)}",
                async ([FromQuery] string key, [FromBody] T entity, [FromServices] TService service) =>
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
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Batch)}",
                async ([FromBody] BatchOperations<T, TKey> operations, [FromServices] TService service) =>
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
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethods.Delete)}",
                async ([FromQuery] string key, [FromServices] TService service) =>
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
            if (type == typeof(string))
                return key => (dynamic)key;
            else if (type == typeof(Guid))
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
            {
                bool hasProperties = type.FetchProperties().Length > 0;
                if (hasProperties)
                    return key => key.FromJson<TKey>();
                else
                    return key => (TKey)Convert.ChangeType(key, type);
            }
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