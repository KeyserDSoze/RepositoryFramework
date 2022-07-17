using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Net;
using System.Reflection;

namespace RepositoryFramework.Infrastructure.Azure.Cosmos.Sql
{
    internal sealed class CosmosSqlRepository<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly Container _client;
        private readonly PropertyInfo[] _properties;
        private readonly ILogger<CosmosSqlRepository<T, TKey>>? _logger;

        public CosmosSqlRepository(CosmosSqlServiceClientFactory clientFactory, ILogger<CosmosSqlRepository<T, TKey>>? logger = null)
        {
            (_client, _properties) = clientFactory.Get(typeof(T).Name);
            _logger = logger;
        }
        private async Task<TReponse> ExecuteAsync<TReponse>(TKey key, RepositoryMethod method, Func<Task<TReponse>> action)
        {
            if (_logger != null)
            {
#pragma warning disable CA2254 // Template should be a static expression
                EventId eventId = new();
                try
                {
                    _logger?.LogInformation(eventId, message: $"{method} for {key}");
                    return await action();
                }
                catch (Exception exception)
                {
                    _logger?.LogError(eventId, message: exception.Message);
                    return default!;
                }
#pragma warning restore CA2254 // Template should be a static expression
            }
            else
                return await action();
        }
        public Task<State> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Delete, async () =>
            {
                var response = await _client.DeleteItemAsync<T>(key.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                return new State(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);
            });


        public Task<State> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Exist, async () =>
                {
                    var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                    return new State(response.StatusCode == HttpStatusCode.OK);
                });

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        => ExecuteAsync(key, RepositoryMethod.Get, async () =>
            {
                var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                if (response.StatusCode == HttpStatusCode.OK)
                    return response.Resource;
                else
                    return default;
            });
        public Task<State> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Insert, async () =>
            {
                var flexible = new ExpandoObject();
                flexible.TryAdd("id", key.ToString());
                foreach (var property in _properties)
                    flexible.TryAdd(property.Name, property.GetValue(value));
                var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                return new State(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created);
            });

        public Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => ExecuteAsync(default!, RepositoryMethod.Query, async () =>
            {
                IQueryable<T> queryable = _client.GetItemLinqQueryable<T>();
                if (options?.Predicate != null)
                    queryable = queryable
                        .Where(options.Predicate);
                if (options?.Order != null)
                    if (options.IsAscending)
                        queryable = queryable.OrderBy(options.Order);
                    else
                        queryable = queryable.OrderByDescending(options.Order);
                if (options?.Skip != null && options.Skip > 0)
                    queryable = queryable.Skip(options.Skip.Value);
                if (options?.Top != null && options.Top > 0)
                    queryable = queryable.Take(options.Top.Value);

                List<T> items = new();
                using (FeedIterator<T> iterator = queryable.ToFeedIterator())
                {
                    while (iterator.HasMoreResults)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return items;
                        foreach (var item in await iterator.ReadNextAsync(cancellationToken).NoContext())
                        {
                            items.Add(item);
                            if (cancellationToken.IsCancellationRequested)
                                return items;
                        }
                    }
                }
                return items.Select(x => x);
            });
        public Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => ExecuteAsync<long>(default!, RepositoryMethod.Count, async () =>
            {
                IQueryable<T> queryable = _client.GetItemLinqQueryable<T>();
                if (options?.Predicate != null)
                    queryable = queryable
                        .Where(options.Predicate);
                if (options?.Order != null)
                    if (options.IsAscending)
                        queryable = queryable.OrderBy(options.Order);
                    else
                        queryable = queryable.OrderByDescending(options.Order);
                if (options?.Skip != null && options.Skip > 0)
                    queryable = queryable.Skip(options.Skip.Value);
                if (options?.Top != null && options.Top > 0)
                    queryable = queryable.Take(options.Top.Value);

                List<T> items = new();
                using (FeedIterator<T> iterator = queryable.ToFeedIterator())
                {
                    while (iterator.HasMoreResults)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return items.Count;
                        foreach (var item in await iterator.ReadNextAsync(cancellationToken).NoContext())
                        {
                            items.Add(item);
                            if (cancellationToken.IsCancellationRequested)
                                return items.Count;
                        }
                    }
                }
                return items.Count;
            });
        public Task<State> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Update, async () =>
            {
                var flexible = new ExpandoObject();
                flexible.TryAdd("id", key.ToString());
                foreach (var property in _properties)
                    flexible.TryAdd(property.Name, property.GetValue(value));
                var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                return new State(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created);
            });
        public async Task<List<BatchResult<TKey, State>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default)
        {
            List<BatchResult<TKey, State>> results = new();
            foreach (var operation in operations)
            {
                switch (operation.Command)
                {
                    case CommandType.Delete:
                        results.Add(new(operation.Command, operation.Key, await DeleteAsync(operation.Key, cancellationToken).NoContext()));
                        break;
                    case CommandType.Insert:
                        results.Add(new(operation.Command, operation.Key, await InsertAsync(operation.Key, operation.Value!, cancellationToken).NoContext()));
                        break;
                    case CommandType.Update:
                        results.Add(new(operation.Command, operation.Key, await UpdateAsync(operation.Key, operation.Value!, cancellationToken).NoContext()));
                        break;
                }
            }
            return results;
        }
    }
}