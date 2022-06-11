namespace RepositoryFramework.Cache
{
    internal sealed class CachedQuery<T> : CachedQuery<T, string>, IQuery<T>, IQueryPattern<T>, IQueryPattern
    {
        public CachedQuery(IQueryPattern<T> query,
            ICache<T>? cache = null,
            CacheOptions<T, string, bool>? cacheOptions = null,
            IDistributedCache<T>? distributed = null,
            DistributedCacheOptions<T, string, bool>? distributedCacheOptions = null) :
            base(query, cache, cacheOptions, distributed, distributedCacheOptions)
        {
        }
    }
}