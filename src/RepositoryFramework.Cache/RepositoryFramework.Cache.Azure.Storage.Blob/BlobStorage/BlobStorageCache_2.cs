namespace RepositoryFramework.Cache.Azure.Storage.Blob
{
    public class BlobStorageCache<T, TKey> : BlobStorageCache<T, TKey, State<T>>, IDistributedCache<T, TKey>
        where TKey : notnull
    {
        public BlobStorageCache(IRepository<BlobStorageCacheModel, string> repository) : base(repository)
        {
        }
    }
}
