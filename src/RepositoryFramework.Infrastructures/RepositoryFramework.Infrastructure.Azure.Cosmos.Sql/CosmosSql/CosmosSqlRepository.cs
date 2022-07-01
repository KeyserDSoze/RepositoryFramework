using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Linq.Expressions;
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
                EventId eventId = new();
                try
                {
                    string message = $"{method} for {key}";
                    _logger?.LogInformation(eventId, message: message);
                    return await action();
                }
                catch (Exception exception)
                {
                    _logger?.LogError(eventId, message: exception.Message);
                    return default!;
                }
            }
            else
                return await action();
        }
        public Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Delete, async () =>
            {
                var response = await _client.DeleteItemAsync<T>(key.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
                return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
            });


        public Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Exist, async () =>
                {
                    var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
                    return response.StatusCode == HttpStatusCode.OK;
                });

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        => ExecuteAsync(key, RepositoryMethod.Get, async () =>
            {
                var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
                if (response.StatusCode == HttpStatusCode.OK)
                    return response.Resource;
                else
                    return default;
            });
        public Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Insert, async () =>
            {
                var flexible = new ExpandoObject();
                flexible.TryAdd("id", key.ToString());
                foreach (var property in _properties)
                    flexible.TryAdd(property.Name, property.GetValue(value));
                var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
                return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created;
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
                        foreach (var item in await iterator.ReadNextAsync(cancellationToken))
                        {
                            items.Add(item);
                            if (cancellationToken.IsCancellationRequested)
                                return items;
                        }
                    }
                }
                return items.Select(x => x);
            });

        public Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(key, RepositoryMethod.Update, async () =>
            {
                var flexible = new ExpandoObject();
                flexible.TryAdd("id", key.ToString());
                foreach (var property in _properties)
                    flexible.TryAdd(property.Name, property.GetValue(value));
                var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
                return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created;
            });
    }
}