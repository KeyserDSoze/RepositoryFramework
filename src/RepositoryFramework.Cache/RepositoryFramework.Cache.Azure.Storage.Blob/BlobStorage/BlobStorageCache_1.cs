namespace RepositoryFramework.Cache.Azure.Storage.Blob
{
    public class BlobStorageCache<T> : BlobStorageCache<T, string>, IDistributedCache<T>
    {
        public BlobStorageCache(IRepository<BlobStorageCacheModel, string> repository) : base(repository)
        {
        }
    }
}
