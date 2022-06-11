namespace RepositoryFramework.Cache.Azure.Storage.Blob
{
    public class BlobStorageCache<T, TKey> : BlobStorageCache<T, TKey, bool>, IDistributedCache<T, TKey>
        where TKey : notnull
    {
        public BlobStorageCache(IRepository<BlobStorageCacheModel, string> repository) : base(repository)
        {
        }
    }
}
