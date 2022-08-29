namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class DefaultTableStorageReader<T, TKey> : ITableStorageEntityReader<T, TKey>
        where TKey : notnull
    {
        public (string PartitionKey, string RowKey, DateTime Timestamp) ReadFromEntity(T? entity) 
            => throw new ArgumentException("Default reader is not allowed to read entity.");
        public (string PartitionKey, string RowKey) ReadFromKey(TKey key)
            => (key?.ToString() ?? string.Empty, string.Empty);
    }
}
