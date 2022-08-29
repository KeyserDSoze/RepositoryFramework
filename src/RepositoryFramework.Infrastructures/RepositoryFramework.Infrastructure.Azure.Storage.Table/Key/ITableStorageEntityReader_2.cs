namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public interface ITableStorageEntityReader<T, TKey>
        where TKey : notnull
    {
        (string PartitionKey, string RowKey) ReadFromKey(TKey key);
    }
}
