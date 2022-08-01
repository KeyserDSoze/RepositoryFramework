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
        private async Task<TReponse> ExecuteAsync<TReponse>(TKey key, RepositoryMethods method, Func<Task<TReponse>> action)
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
        public Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethods.Delete, async () =>
            {
                var response = await _client.DeleteItemAsync<T>(key.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                return new State<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);
            });


        public Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethods.Exist, async () =>
                {
                    var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                    return new State<T>(response.StatusCode == HttpStatusCode.OK);
                });

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        => ExecuteAsync(key, RepositoryMethods.Get, async () =>
            {
                var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                if (response.StatusCode == HttpStatusCode.OK)
                    return response.Resource;
                else
                    return default;
            });
        public Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethods.Insert, async () =>
            {
                var flexible = new ExpandoObject();
                flexible.TryAdd("id", key.ToString());
                foreach (var property in _properties)
                    flexible.TryAdd(property.Name, property.GetValue(value));
                var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                return new State<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created, value);
            });

        public Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => ExecuteAsync(default!, RepositoryMethods.Query, async () =>
            {
                IQueryable<T> queryable = _client.GetItemLinqQueryable<T>().Filter(options);

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
            => ExecuteAsync<long>(default!, RepositoryMethods.Count, async () =>
            {
                IQueryable<T> queryable = _client.GetItemLinqQueryable<T>().Filter(options);

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
        public Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethods.Update, async () =>
            {
                var flexible = new ExpandoObject();
                flexible.TryAdd("id", key.ToString());
                foreach (var property in _properties)
                    flexible.TryAdd(property.Name, property.GetValue(value));
                var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
                return new State<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created, value);
            });
        public async Task<BatchResults<TKey, State<T>>> BatchAsync(BatchOperations<T, TKey, State<T>> operations, CancellationToken cancellationToken = default)
        {
            BatchResults<TKey, State<T>> results = new();
            foreach (var operation in operations.Values)
            {
                switch (operation.Command)
                {
                    case CommandType.Delete:
                        results.AddDelete(operation.Key, await DeleteAsync(operation.Key, cancellationToken).NoContext());
                        break;
                    case CommandType.Insert:
                        results.AddInsert(operation.Key, await InsertAsync(operation.Key, operation.Value!, cancellationToken).NoContext());
                        break;
                    case CommandType.Update:
                        results.AddUpdate(operation.Key, await UpdateAsync(operation.Key, operation.Value!, cancellationToken).NoContext());
                        break;
                }
            }
            return results;
        }
    }
}