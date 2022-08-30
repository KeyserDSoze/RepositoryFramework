namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class DefaultTableStorageKeyReader<T> : DefaultTableStorageKeyReader<T, string>, ITableStorageKeyReader<T> { }
}
