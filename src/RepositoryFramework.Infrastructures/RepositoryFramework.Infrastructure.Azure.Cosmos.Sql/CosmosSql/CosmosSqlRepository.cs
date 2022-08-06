using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RepositoryFramework.Infrastructure.Azure.Cosmos.Sql
{
    internal sealed class CosmosSqlRepository<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly Container _client;
        private readonly PropertyInfo[] _properties;

        public CosmosSqlRepository(CosmosSqlServiceClientFactory clientFactory)
        {
            (_client, _properties) = clientFactory.Get(typeof(T).Name);
        }
        public async Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteItemAsync<T>(key.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
            return new State<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);
        }

        public async Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
            return new State<T>(response.StatusCode == HttpStatusCode.OK);
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.ReadItemAsync<T>(key!.ToString(), new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
            if (response.StatusCode == HttpStatusCode.OK)
                return response.Resource;
            else
                return default;
        }
        public async Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var flexible = new ExpandoObject();
            flexible.TryAdd("id", key.ToString());
            foreach (var property in _properties)
                flexible.TryAdd(property.Name, property.GetValue(value));
            var response = await _client.CreateItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
            return new State<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created, value);
        }

        public async IAsyncEnumerable<T> QueryAsync(QueryOptions<T>? options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            IQueryable<T> queryable = _client.GetItemLinqQueryable<T>().Filter(options);

            using (FeedIterator<T> iterator = queryable.ToFeedIterator())
            {
                while (iterator.HasMoreResults)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    foreach (var item in await iterator.ReadNextAsync(cancellationToken).NoContext())
                        yield return item;
                }
            }
        }
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            QueryOptions<T>? options = null,
            Expression<Func<T, TProperty>>? aggregateExpression = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> queryable = _client.GetItemLinqQueryable<T>().Filter(options);
            return operation.ExecuteAsync(
                () => queryable.CountAsync(cancellationToken),
                () => queryable.Sum(x => (decimal)(object)aggregateExpression!.Compile().Invoke(x)!),
                async () => (await queryable.Select(aggregateExpression!).AsQueryable().MaxAsync(cancellationToken).NoContext()).Resource,
                async () => (await queryable.Select(aggregateExpression!).AsQueryable().MinAsync(cancellationToken).NoContext()).Resource,
                () => queryable.Average(x => (decimal)(object)aggregateExpression!.Compile().Invoke(x)!)
                );
        }
        public async Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var flexible = new ExpandoObject();
            flexible.TryAdd("id", key.ToString());
            foreach (var property in _properties)
                flexible.TryAdd(property.Name, property.GetValue(value));
            var response = await _client.UpsertItemAsync(flexible, new PartitionKey(key.ToString()), cancellationToken: cancellationToken).NoContext();
            return new State<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created, value);
        }
        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            BatchResults<T, TKey> results = new();
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