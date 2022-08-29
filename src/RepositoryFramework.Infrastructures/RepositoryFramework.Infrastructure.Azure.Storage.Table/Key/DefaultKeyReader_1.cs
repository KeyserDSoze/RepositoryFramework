namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class DefaultKeyReader<T> : DefaultKeyReader<T, string>, ITableStorageKeyReader<T> { }
}
