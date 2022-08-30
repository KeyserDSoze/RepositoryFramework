namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class TableStorageRepository<T> : TableStorageRepository<T, string>, IRepositoryPattern<T>
    {
        public TableStorageRepository(TableServiceClientFactory clientFactory,
            ITableStorageKeyReader<T> keyReader,
            TableStorageOptions<T, string> options) : base(clientFactory, keyReader, options)
        {
        }
    }
}