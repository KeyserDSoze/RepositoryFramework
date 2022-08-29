namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class DefaultKeyReader<T, TKey> : ITableStorageKeyReader<T, TKey>
        where TKey : notnull
    {
        public (string PartitionKey, string RowKey) Read(TKey key, T? entity)
            => (key?.ToString() ?? string.Empty, string.Empty);
    }
}
