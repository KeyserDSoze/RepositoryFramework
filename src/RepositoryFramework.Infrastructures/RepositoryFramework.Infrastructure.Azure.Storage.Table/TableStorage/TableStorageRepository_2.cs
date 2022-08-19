using Azure.Data.Tables;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class TableStorageRepository<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly TableClient _client;
        public TableStorageRepository(TableServiceClientFactory clientFactory)
        {
            _client = clientFactory.Get(typeof(T).Name);
        }
        private sealed class TableEntity : ITableEntity
        {
            public string PartitionKey { get; set; } = null!;
            public string RowKey { get; set; } = null!;
            public DateTimeOffset? Timestamp { get; set; }
            public string Value { get; set; } = null!;
            public global::Azure.ETag ETag { get; set; }
        }
        public async Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteEntityAsync(key!.ToString(), string.Empty, cancellationToken: cancellationToken).NoContext();
            return !response.IsError;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetEntityAsync<TableEntity>(key!.ToString(), string.Empty, cancellationToken: cancellationToken).NoContext();
            if (response?.Value != null)
                return JsonSerializer.Deserialize<T>(response.Value.Value);
            return default;
        }
        public async Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetEntityAsync<TableEntity>(key!.ToString(), string.Empty, cancellationToken: cancellationToken).NoContext();
            return response?.Value != null;
        }

        public Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => UpdateAsync(key, value, cancellationToken);

        public async IAsyncEnumerable<T> QueryAsync(Query query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            Func<T, bool> predicate = x => true;
#warning to check well, and check if possible to get a queryable item to improve query request
            LambdaExpression? where = (query.Operations.FirstOrDefault(x => x.Operation == QueryOperations.Where) as LambdaQueryOperation)?.Expression;
            if (where != null)
                predicate = where.AsExpression<T, bool>().Compile();
            await foreach (var entity in _client.QueryAsync<TableEntity>(cancellationToken: cancellationToken))
            {
                var item = JsonSerializer.Deserialize<T>(entity.Value)!;
                if (!predicate.Invoke(item))
                    continue;
                yield return item;
            }
        }
        public async ValueTask<TProperty> OperationAsync<TProperty>(
          OperationType<TProperty> operation,
          Query query,
          CancellationToken cancellationToken = default)
        {
            List<T> items = new();
            await foreach (var item in QueryAsync(query, cancellationToken))
                items.Add(item);
            LambdaExpression? select = query.FirstSelect;
            return (await operation.ExecuteAsync(
                () => ValueTask.FromResult((TProperty)(object)items.Count),
                () => ValueTask.FromResult((TProperty)(object)items.Sum(x => select!.InvokeAndTransform<decimal>(x!))),
                () => ValueTask.FromResult((TProperty)(object)items.Max(x => select!.InvokeAndTransform<decimal>(x!))),
                () => ValueTask.FromResult((TProperty)(object)items.Min(x => select!.InvokeAndTransform<decimal>(x!))),
                () => ValueTask.FromResult((TProperty)(object)items.Average(x => select!.InvokeAndTransform<decimal>(x!)))
                ))!;
        }
        public async Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var response = await _client.UpsertEntityAsync(new TableEntity
            {
                PartitionKey = key!.ToString()!,
                RowKey = string.Empty,
                Value = JsonSerializer.Serialize(value)
            }, TableUpdateMode.Replace, cancellationToken).NoContext();
            return !response.IsError;
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