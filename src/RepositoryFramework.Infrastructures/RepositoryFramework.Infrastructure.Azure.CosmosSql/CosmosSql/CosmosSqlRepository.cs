using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace RepositoryFramework.Infrastructure.Azure.CosmosSql
{
    internal sealed class CosmosSqlRepository<T, TKey> : IRepository<T, TKey>
        where TKey : notnull
    {
        private readonly Container _client;
        private readonly PropertyInfo[] _properties;
        public CosmosSqlRepository(CosmosSqlServiceClientFactory clientFactory)
        {
            (_client, _properties) = clientFactory.Get(typeof(T).Name);
        }
        public async Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteItemAsync<T>(key.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
                return response.Resource;
            else
                return default;
        }
        public async Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var flexible = new ExpandoObject();
            flexible.TryAdd("id", key.ToString());
            foreach (var property in _properties)
                flexible.TryAdd(property.Name, property.GetValue(value));
            var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> queryable = _client.GetItemLinqQueryable<T>();
            if (predicate != null)
                queryable = queryable
                    .Where(predicate);
            if (top != null && top > 0)
                queryable = queryable.Take(top.Value);
            if (skip != null && skip > 0)
                queryable = queryable.Skip(skip.Value);
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
            return items;
        }

        public async Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var flexible = new ExpandoObject();
            flexible.TryAdd("id", key.ToString());
            foreach (var property in _properties)
                flexible.TryAdd(property.Name, property.GetValue(value));
            var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken);
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created;
        }
    }
}