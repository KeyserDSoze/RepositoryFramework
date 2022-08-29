namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public interface ITableStorageKeyReader<T, TKey>
        where TKey : notnull
    {
        (string PartitionKey, string RowKey) Read(TKey key, T? entity);
    }
}
