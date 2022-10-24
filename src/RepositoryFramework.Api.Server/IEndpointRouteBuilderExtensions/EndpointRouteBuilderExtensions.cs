﻿using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class EndpointRouteBuilderExtensions
    {
        private static readonly Dictionary<string, bool> SetupRepositories = new();
        /// <summary>
        /// Add repository or CQRS service injected as api for your <typeparamref name="T"/> model.
        /// </summary>
        /// <typeparam name="T">Model of your repository or CQRS that you want to add as api</typeparam>
        /// <param name="app">IEndpointRouteBuilder</param>
        /// <returns>ApiAuthorizationBuilder</returns>
        public static IApiAuthorizationBuilder UseApiFromRepository<T>(this IEndpointRouteBuilder app)
            => new ApiAuthorizationBuilder(authorization => app.UseApiFromRepository(typeof(T), ApiSettings.Instance, authorization));

        /// <summary>
        /// Add all repository or CQRS services injected as api.
        /// </summary>
        /// <typeparam name="TEndpointRouteBuilder"></typeparam>
        /// <param name="app">IEndpointRouteBuilder</param>
        /// <param name="startingPath">By default is "api", but you can choose your path. https://{your domain}/{startingPath}</param>
        /// <returns>ApiAuthorizationBuilder</returns>
        public static IApiAuthorizationBuilder UseApiFromRepositoryFramework<TEndpointRouteBuilder>(
            this TEndpointRouteBuilder app)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        => new ApiAuthorizationBuilder(authorization =>
            {
                var services = app.ServiceProvider.GetService<RepositoryFrameworkRegistry>();
                foreach (var service in services!.Services)
                    _ = app.UseApiFromRepository(service.ModelType, ApiSettings.Instance, authorization);
                return app;
            });

        private const string NotImplementedExceptionIlOperation = "newobj instance void System.NotImplementedException";
        private static readonly List<string> s_possibleMethods = new()
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
        private static TEndpointRouteBuilder UseApiFromRepository<TEndpointRouteBuilder>(
            this TEndpointRouteBuilder app,
            Type modelType,
            ApiSettings settings,
            ApiAuthorization? authorization)
            where TEndpointRouteBuilder : IEndpointRouteBuilder
        {
            if (SetupRepositories.ContainsKey(modelType.FullName!))
                return app;
            SetupRepositories.Add(modelType.FullName!, true);
            var registry = app.ServiceProvider.GetService<RepositoryFrameworkRegistry>();
            var serviceValue = registry!.Services.FirstOrDefault(x => x.ModelType == modelType);
            if (serviceValue == null)
                throw new ArgumentException($"Please check if your {modelType.Name} model has a service injected for IRepository, IQuery, ICommand.");
            var currentName = modelType.Name;
            if (settings.Names.ContainsKey(modelType.FullName!))
                currentName = settings.Names[modelType.FullName!];
            if (app is IApplicationBuilder applicationBuilder)
            {
                if (ApiSettings.Instance.HasSwagger && !ApiSettings.Instance.SwaggerInstalled)
                {
                    applicationBuilder.UseSwaggerUiForRepository(ApiSettings.Instance);
                    ApiSettings.Instance.SwaggerInstalled = true;
                }
                if (ApiSettings.Instance.HasDefaultCors && !ApiSettings.Instance.CorsInstalled)
                {
                    applicationBuilder.UseCors(ApiSettings.AllowSpecificOrigins);
                    ApiSettings.Instance.CorsInstalled = true;
                }
            }
            Dictionary<string, bool> configuredMethods = new();
            if (app.ServiceProvider.GetService(typeof(RepositoryFrameworkOptions<,>).MakeGenericType(serviceValue.ModelType, serviceValue.KeyType)) is IRepositoryFrameworkOptions options && !options.IsNotExposableAsApi)
            {
                foreach (var (interfaceType, currentType) in serviceValue.RepositoryTypes.Select(x => x.Value))
                    foreach (var method in currentType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var currentMethodName = s_possibleMethods.FirstOrDefault(x => x == $"Add{method.Name.Replace("Async", string.Empty)}");
                        if (!string.IsNullOrWhiteSpace(currentMethodName) && !configuredMethods.ContainsKey(currentMethodName))
                        {
                            var isNotImplemented = false;
                            Try.WithDefaultOnCatch(() =>
                            {
                                var instructions = method.GetBodyAsString();
                                isNotImplemented = instructions.Contains(NotImplementedExceptionIlOperation);
                            });
                            if (isNotImplemented)
                                continue;

                            _ = typeof(EndpointRouteBuilderExtensions).GetMethod(currentMethodName, BindingFlags.NonPublic | BindingFlags.Static)!
                               .MakeGenericMethod(modelType, serviceValue.KeyType, interfaceType)
                               .Invoke(null, new object[] { app, currentName, settings.StartingPath, authorization! });
                            configuredMethods.Add(currentMethodName, true);
                        }
                    }
            }
            return app;
        }
        private static void AddGet<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
           where TKey : notnull
            => app.AddApi<T, TKey, TService, T?>(name, startingPath, authorization,
                RepositoryMethods.Get, null,
                (TKey key, TService service) =>
                {
                    var queryService = service as IQueryPattern<T, TKey>;
                    return queryService!.GetAsync(key);
                });
        private static void AddExist<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
           where TKey : notnull
           => app.AddApi<T, TKey, TService, State<T, TKey>>(name, startingPath, authorization,
               RepositoryMethods.Exist, null,
               (TKey key, TService service) =>
               {
                   var queryService = service as IQueryPattern<T, TKey>;
                   return queryService!.ExistAsync(key);
               }
           );
#warning think about to have more than one api with the same name and a dictionary? Problem with change of mind? the key name can be used?
        private static void AddQuery<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Query)}",
                async ([FromBody] SerializableFilter? serializableFilter, [FromServices] TService service) =>
                {
                    var filter = (serializableFilter ?? SerializableFilter.Empty).Deserialize<T>();
                    var queryService = service as IQueryPattern<T, TKey>;
                    return await queryService!.QueryAsync(filter).ToListAsync().NoContext();
                }).WithName($"{nameof(RepositoryMethods.Query)}{name}")
              .AddAuthorization(authorization, RepositoryMethods.Query);
        }
        private static void AddOperation<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethods.Operation)}",
                async ([FromQuery] string op, [FromQuery] string? returnType,
                    [FromBody] SerializableFilter? serializableFilter, [FromServices] TService service) =>
                {
                    var filter = (serializableFilter ?? SerializableFilter.Empty).Deserialize<T>();
                    var type = CalculateTypeFromQuery();
                    var queryService = service as IQueryPattern<T, TKey>;
                    var result = await Generics.WithStatic(
                                  typeof(EndpointRouteBuilderExtensions),
                                  nameof(GetResultFromOperation),
                                  typeof(T), typeof(TKey), type)
                                .InvokeAsync(queryService!, op, filter)!;
                    return result;

                    Type CalculateTypeFromQuery()
                    {
                        var calculatedType = typeof(object);
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
            string operationName,
            IFilterExpression filter)
            where TKey : notnull
            => queryService.OperationAsync(
                new OperationType<TProperty>(operationName),
                filter);
        private static void AddInsert<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            => app.AddApi(name, startingPath, authorization,
                RepositoryMethods.Insert,
                (T entity, TKey key, TService service) =>
                {
                    var commandService = service as ICommandPattern<T, TKey>;
                    return commandService!.InsertAsync(key, entity);
                },
                null
            );
        private static void AddUpdate<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            => app.AddApi(name, startingPath, authorization,
                RepositoryMethods.Update,
                (T entity, TKey key, TService service) =>
                {
                    var commandService = service as ICommandPattern<T, TKey>;
                    return commandService!.UpdateAsync(key, entity);
                },
                null
            );
        private static void AddDelete<T, TKey, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            => app.AddApi<T, TKey, TService, State<T, TKey>>(name, startingPath, authorization,
                RepositoryMethods.Delete, null,
                (TKey key, TService service) =>
                {
                    var commandService = service as ICommandPattern<T, TKey>;
                    return commandService!.DeleteAsync(key);
                }
            );
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
        private static void AddApi<T, TKey, TService, TResult>(this IEndpointRouteBuilder app,
            string name,
            string startingPath,
            ApiAuthorization? authorization,
            RepositoryMethods method,
            Func<T, TKey, TService, Task<TResult>>? action,
            Func<TKey, TService, Task<TResult>>? actionWithNoEntity)
            where TKey : notnull
        {
            var parser = GetKeyParser<TKey>();
            var keyType = typeof(TKey);
            RouteHandlerBuilder? apiMapped = null;
            if (IKey.IsJsonable(keyType) && actionWithNoEntity != null)
            {
                apiMapped = app.MapPost($"{startingPath}/{name}/{method}",
                ([FromBody] TKey key, [FromServices] TService service)
                    => actionWithNoEntity.Invoke(key, service));
            }
            else if (IKey.IsJsonable(keyType) && action != null)
            {
                apiMapped = app.MapPost($"{startingPath}/{name}/{method}",
                ([FromBody] Entity<T, TKey> entity, [FromServices] TService service)
                    => action.Invoke(entity.Value!, entity.Key!, service));
            }
            else if (!IKey.IsJsonable(keyType) && action != null)
            {
                apiMapped = app.MapPost($"{startingPath}/{name}/{method}",
                ([FromQuery] string key, [FromBody] T entity, [FromServices] TService service)
                    => action.Invoke(entity, parser(key), service));
            }
            else
            {
                apiMapped = app.MapGet($"{startingPath}/{name}/{method}",
                    ([FromQuery] string key, [FromServices] TService service)
                        => actionWithNoEntity!.Invoke(parser(key), service));
            }
            _ = apiMapped!
                    .WithName($"{method}{name}")
                    .AddAuthorization(authorization, method);
        }
        private static Func<string, TKey> GetKeyParser<TKey>()
        {
            var type = typeof(TKey);
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
                var hasProperties = type.FetchProperties().Length > 0;
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
