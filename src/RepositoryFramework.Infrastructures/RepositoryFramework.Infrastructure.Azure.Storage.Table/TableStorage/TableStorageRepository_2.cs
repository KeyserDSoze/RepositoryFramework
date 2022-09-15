using Azure.Data.Tables;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class TableStorageRepository<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly TableClient _client;
        private readonly ITableStorageKeyReader<T, TKey> _keyReader;
        private readonly TableStorageOptions<T, TKey> _options;

        public TableStorageRepository(TableServiceClientFactory clientFactory,
            ITableStorageKeyReader<T, TKey> keyReader,
            TableStorageOptions<T, TKey> options)
        {
            _client = clientFactory.Get(typeof(T).Name);
            _keyReader = keyReader;
            _options = options;
        }
        private sealed class TableEntity : ITableEntity
        {
            public string PartitionKey { get; set; } = null!;
            public string RowKey { get; set; } = null!;
            public DateTimeOffset? Timestamp { get; set; }
            public string Value { get; set; } = null!;
            public global::Azure.ETag ETag { get; set; }
        }

        public async Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var realKey = _keyReader.Read(key);
            var response = await _client.DeleteEntityAsync(realKey.PartitionKey, realKey.RowKey, cancellationToken: cancellationToken).NoContext();
            return !response.IsError;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var realKey = _keyReader.Read(key);
            var response = await _client.GetEntityAsync<TableEntity>(realKey.PartitionKey, realKey.RowKey, cancellationToken: cancellationToken).NoContext();
            if (response?.Value != null)
                return JsonSerializer.Deserialize<T>(response.Value.Value);
            return default;
        }
        public async Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var realKey = _keyReader.Read(key);
            await foreach (var entity in _client.QueryAsync<TableEntity>(
                filter: $"PartitionKey eq '{realKey.PartitionKey}' and RowKey eq '{realKey.RowKey}'", 1, cancellationToken: cancellationToken))
                return new State<T>(true, JsonSerializer.Deserialize<T>(entity.Value));
            return false;
        }

        public Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => UpdateAsync(key, value, cancellationToken);

        public async IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(Query query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            LambdaExpression? where = (query.Operations.FirstOrDefault(x => x.Operation == QueryOperations.Where) as LambdaQueryOperation)?.Expression;
            string? filter = null;
            if (where != null)
                filter = QueryStrategy.Create(where.Body, _options.PartitionKey.Name, _options.RowKey.Name, _options.Timestamp?.Name);

            long? top = (query.Operations.FirstOrDefault(x => x.Operation == QueryOperations.Top) as ValueQueryOperation)?.Value;
            long? skip = (query.Operations.FirstOrDefault(x => x.Operation == QueryOperations.Skip) as ValueQueryOperation)?.Value;
            int counter = 0;
            var items = new List<T>();

            await foreach (var page in _client.QueryAsync<TableEntity>(filter: filter,
                maxPerPage: 50,
                cancellationToken: cancellationToken).AsPages())
            {
                bool haveToBreak = false;
                foreach (var entity in page.Values)
                {
                    counter++;
                    if (skip != null && counter <= skip)
                        continue;
                    haveToBreak = top != null && counter > top + (skip ?? 0);
                    if (haveToBreak)
                        break;
                    var item = JsonSerializer.Deserialize<T>(entity.Value)!;
                    items.Add(item);
                }
                if (haveToBreak)
                    break;
            }
            if (!cancellationToken.IsCancellationRequested)
                foreach (var item in Filter(items.AsQueryable(), query))
                    yield return IEntity.Default(_keyReader.Read(item), item);
        }
        private static IQueryable<T> Filter(IQueryable<T> queryable, Query query)
        {
            foreach (var operation in query.Operations)
            {
                if (operation is LambdaQueryOperation lambda)
                {
                    queryable = lambda.Operation switch
                    {
                        QueryOperations.Where => queryable.Where(lambda.Expression!.AsExpression<T, bool>()).AsQueryable(),
                        QueryOperations.OrderBy => queryable.OrderBy(lambda.Expression!),
                        QueryOperations.OrderByDescending => queryable.OrderByDescending(lambda.Expression!),
                        QueryOperations.ThenBy => (queryable as IOrderedQueryable<T>)!.ThenBy(lambda.Expression!),
                        QueryOperations.ThenByDescending => (queryable as IOrderedQueryable<T>)!.ThenByDescending(lambda.Expression!),
                        _ => queryable,
                    };
                }
            }
            return queryable;
        }
        public async ValueTask<TProperty> OperationAsync<TProperty>(
          OperationType<TProperty> operation,
          Query query,
          CancellationToken cancellationToken = default)
        {
            List<T> items = new();
            await foreach (var item in QueryAsync(query, cancellationToken))
                items.Add(item.Value);
            var selected = query.FilterAsSelect(items);
            return (await operation.ExecuteAsync(
                () => Invoke<TProperty>(selected.Count()),
                () => Invoke<TProperty>(selected.Sum(x => ((object)x).Cast<decimal>())),
                () => Invoke<TProperty>(selected.Max()!),
                () => Invoke<TProperty>(selected.Min()!),
                () => Invoke<TProperty>(selected.Average(x => ((object)x).Cast<decimal>()))))!;
        }
        private static ValueTask<TProperty> Invoke<TProperty>(object value)
            => ValueTask.FromResult((TProperty)Convert.ChangeType(value, typeof(TProperty)));
        public async Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var realKey = _keyReader.Read(key);
            var response = await _client.UpsertEntityAsync(new TableEntity
            {
                PartitionKey = realKey.PartitionKey,
                RowKey = realKey.RowKey,
                Value = JsonSerializer.Serialize(value)
            }, TableUpdateMode.Replace, cancellationToken).NoContext();
            return new State<T>(!response.IsError, value);
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
