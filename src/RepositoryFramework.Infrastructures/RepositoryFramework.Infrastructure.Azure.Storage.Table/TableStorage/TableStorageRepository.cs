using Azure.Data.Tables;
using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.TableStorage
{
    internal sealed class TableStorageRepository<T, TKey> : IRepositoryPattern<T, TKey>, IQueryPattern<T, TKey>, ICommandPattern<T, TKey>
        where TKey : notnull
    {
        private readonly TableClient _client;
        public TableStorageRepository(TableServiceClientFactory clientFactory)
        {
            _client = clientFactory.Get(typeof(T).Name);
        }
        private sealed class TableEntity : ITableEntity
        {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public string Value { get; set; }
            public global::Azure.ETag ETag { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        }
        public async Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteEntityAsync(key!.ToString(), string.Empty, cancellationToken: cancellationToken);
            return !response.IsError;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetEntityAsync<TableEntity>(key!.ToString(), string.Empty, cancellationToken: cancellationToken);
            if (response?.Value != null)
                return JsonSerializer.Deserialize<T>(response.Value.Value);
            return default;
        }

        public Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => UpdateAsync(key, value, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
            => throw new NotImplementedException("It's not easy to do with tablestorage. If you want to have a query more complex that only the partition key you have to implement another storage. TableStorage is not the right solution for you.");

        public async Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var response = await _client.UpsertEntityAsync(new TableEntity
            {
                PartitionKey = key!.ToString()!,
                RowKey = string.Empty,
                Value = JsonSerializer.Serialize(value)
            }, TableUpdateMode.Replace, cancellationToken);
            return !response.IsError;
        }
    }
}