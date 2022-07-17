using Azure.Data.Tables;
using System.Linq.Expressions;
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
        public async Task<State> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
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
        public async Task<State> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetEntityAsync<TableEntity>(key!.ToString(), string.Empty, cancellationToken: cancellationToken).NoContext();
            return response?.Value != null;
        }

        public Task<State> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => UpdateAsync(key, value, cancellationToken);

        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            List<T> entities = new();
            await foreach (var item in _client.QueryAsync<TableEntity>(cancellationToken: cancellationToken))
            {
                entities.Add(JsonSerializer.Deserialize<T>(item.Value)!);
            }
            IEnumerable<T> results = entities.Filter(options);
            return results;
        }
        public async Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            List<T> entities = new();
            await foreach (var item in _client.QueryAsync<TableEntity>(cancellationToken: cancellationToken))
            {
                entities.Add(JsonSerializer.Deserialize<T>(item.Value)!);
            }
            IEnumerable<T> results = entities.Filter(options);
            return results.Count();
        }
        public async Task<State> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var response = await _client.UpsertEntityAsync(new TableEntity
            {
                PartitionKey = key!.ToString()!,
                RowKey = string.Empty,
                Value = JsonSerializer.Serialize(value)
            }, TableUpdateMode.Replace, cancellationToken).NoContext();
            return !response.IsError;
        }

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