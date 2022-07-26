using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class EndpointRouteBuilderExtensions
    {
        private static void AddGet<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Get)}", async (string key, [FromServices] TService service) =>
               {
                   var queryService = service as IQueryPattern<T, TKey, TState>;
                   var keyAsValue = parser(key);
                   return await queryService!.GetAsync(keyAsValue).NoContext();
               }).WithName($"{nameof(RepositoryMethod.Get)}{name}")
               .AddAuthorization(authorization, RepositoryMethod.Get);
        }
        private static void AddQuery<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Query)}",
                async (string? query, int? top, int? skip, string? order, bool? asc, [FromServices] TService service) =>
              {
                  var options = QueryOptions<T>.ComposeFromQuery(query, top, skip, order, asc);
                  var queryService = service as IQueryPattern<T, TKey, TState>;
                  return await queryService!.QueryAsync(options).NoContext();

              }).WithName($"{nameof(RepositoryMethod.Query)}{name}")
              .AddAuthorization(authorization, RepositoryMethod.Query);
        }
        private static void AddCount<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Count)}",
                async (string? query, int? top, int? skip, string? order, bool? asc, [FromServices] TService service) =>
                {
                    var options = QueryOptions<T>.ComposeFromQuery(query, top, skip, order, asc);
                    var queryService = service as IQueryPattern<T, TKey, TState>;
                    return await queryService!.CountAsync(options).NoContext();

                }).WithName($"{nameof(RepositoryMethod.Count)}{name}")
              .AddAuthorization(authorization, RepositoryMethod.Count);
        }
        private static void AddExist<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Exist)}", async (string key, [FromServices] TService service) =>
            {
                var queryService = service as IQueryPattern<T, TKey, TState>;
                var keyAsValue = parser(key);
                return await queryService!.ExistAsync(keyAsValue).NoContext();
            }).WithName($"{nameof(RepositoryMethod.Exist)}{name}")
               .AddAuthorization(authorization, RepositoryMethod.Exist);
        }
        private static void AddInsert<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethod.Insert)}", async (string key, T entity, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey, TState>;
                var keyAsValue = parser(key);
                return await commandService!.InsertAsync(keyAsValue, entity).NoContext();
            }).WithName($"{nameof(RepositoryMethod.Insert)}{name}")
            .AddAuthorization(authorization, RepositoryMethod.Insert);
        }
        private static void AddUpdate<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethod.Update)}", async (string key, T entity, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey, TState>;
                var keyAsValue = parser(key);
                return await commandService!.UpdateAsync(keyAsValue, entity).NoContext();
            }).WithName($"{nameof(RepositoryMethod.Update)}{name}")
            .AddAuthorization(authorization, RepositoryMethod.Update);
        }
        private static void AddBatch<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            _ = app.MapPost($"{startingPath}/{name}/{nameof(RepositoryMethod.Batch)}", async (BatchOperations<T, TKey, TState> operations, [FromServices] TService service) =>
            {
                var commandService = service as ICommandPattern<T, TKey, TState>;
                return await commandService!.BatchAsync(operations).NoContext();
            }).WithName($"{nameof(RepositoryMethod.Batch)}{name}")
            .AddAuthorization(authorization, RepositoryMethod.Batch);
        }
        private static void AddDelete<T, TKey, TState, TService>(IEndpointRouteBuilder app, string name, string startingPath, ApiAuthorization? authorization)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            var parser = GetKeyParser<TKey>();
            _ = app.MapGet($"{startingPath}/{name}/{nameof(RepositoryMethod.Delete)}", async (string key, [FromServices] TService service) =>
                {
                    var commandService = service as ICommandPattern<T, TKey, TState>;
                    var keyAsValue = parser(key);
                    return await commandService!.DeleteAsync(keyAsValue).NoContext();
                }).WithName($"{nameof(RepositoryMethod.Delete)}{name}")
                .AddAuthorization(authorization, RepositoryMethod.Delete);
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
    }
}
