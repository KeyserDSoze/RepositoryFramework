namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class DefaultTableStorageKeyReader<T, TKey> : ITableStorageKeyReader<T, TKey>
        where TKey : notnull
    {
        public (string PartitionKey, string RowKey) Read(TKey key)
            => (key?.ToString() ?? string.Empty, string.Empty);
    }
}
