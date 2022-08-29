namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class DefaultTableStorageReader<T> : DefaultTableStorageReader<T, string>, ITableStorageEntityReader<T> { }
}
