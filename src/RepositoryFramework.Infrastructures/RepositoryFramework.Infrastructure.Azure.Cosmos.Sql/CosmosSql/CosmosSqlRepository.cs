using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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
        private readonly CosmosOptions<T, TKey> _settings;

        public CosmosSqlRepository(CosmosSqlServiceClientFactory clientFactory, CosmosOptions<T, TKey> settings)
        {
            (_client, _properties) = clientFactory.Get(typeof(T).Name);
            _settings = settings;
        }
        private static string GetKeyAsString(TKey key)
        {
            if (key is IKey customKey)
                return customKey.AsString();
            return key.ToString()!;
        }
        public async Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var keyAsString = GetKeyAsString(key);
            var response = await _client.DeleteItemAsync<T>(keyAsString, new PartitionKey(keyAsString), cancellationToken: cancellationToken).NoContext();
            return IState.Default<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);
        }

        public async Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var keyAsString = GetKeyAsString(key);
            var parameterizedQuery = new QueryDefinition(
                query: _settings.ExistQuery
            )
            .WithParameter("@id", keyAsString);
            using var filteredFeed = _client.GetItemQueryIterator<T>(queryDefinition: parameterizedQuery);
            var response = await filteredFeed.ReadNextAsync(cancellationToken);
            return IState.Default<T>(response.Any());
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var keyAsString = GetKeyAsString(key);
            var response = await _client.ReadItemAsync<T>(keyAsString, new PartitionKey(keyAsString), cancellationToken: cancellationToken).NoContext();
            if (response.StatusCode == HttpStatusCode.OK)
                return response.Resource;
            else
                return default;
        }
        public async Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var keyAsString = GetKeyAsString(key);
            var flexible = new ExpandoObject();
            flexible.TryAdd("id", keyAsString);
            foreach (var property in _properties)
                flexible.TryAdd(property.Name, property.GetValue(value));
            var response = await _client.CreateItemAsync(flexible, new PartitionKey(keyAsString), cancellationToken: cancellationToken).NoContext();
            return IState.Default(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created, value);
        }

        public async IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(Query query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var queryable = query.Filter(_client.GetItemLinqQueryable<T>());

            using var iterator = queryable.ToFeedIterator();
            while (iterator.HasMoreResults)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
#warning to retrieve in some way the key
                foreach (var item in await iterator.ReadNextAsync(cancellationToken).NoContext())
                    yield return IEntity.Default(default(TKey)!, item);
            }
        }
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            Query query,
            CancellationToken cancellationToken = default)
        {
            var queryable = query.Filter(_client.GetItemLinqQueryable<T>());
            var select = query.FirstSelect;
            return operation.ExecuteAsync(
                () => queryable.CountAsync(cancellationToken)!,
                () => queryable.Sum(x => select!.InvokeAndTransform<decimal>(x!)!),
                async () => (await queryable.Select(x => select!.InvokeAndTransform<object>(x!)).AsQueryable().MaxAsync(cancellationToken).NoContext()).Resource,
                async () => (await queryable.Select(x => select!.InvokeAndTransform<object>(x!)).AsQueryable().MinAsync(cancellationToken).NoContext()).Resource,
                () => queryable.Average(x => select!.InvokeAndTransform<decimal>(x!))
                )!;
        }
        public async Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var keyAsString = GetKeyAsString(key);
            var flexible = new ExpandoObject();
            flexible.TryAdd("id", keyAsString);
            foreach (var property in _properties)
                flexible.TryAdd(property.Name, property.GetValue(value));
            var response = await _client.UpsertItemAsync(flexible, new PartitionKey(keyAsString), cancellationToken: cancellationToken).NoContext();
            return IState.Default<T>(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created, value);
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
