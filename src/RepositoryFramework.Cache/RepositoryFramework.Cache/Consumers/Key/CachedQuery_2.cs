namespace RepositoryFramework.Cache
{
    internal class CachedQuery<T, TKey> : CachedQuery<T, TKey, bool>, IQuery<T, TKey>
         where TKey : notnull
    {
        public CachedQuery(IQueryPattern<T, TKey> query,
            ICache<T, TKey>? cache = null,
            CacheOptions<T, TKey, bool>? cacheOptions = null,
            IDistributedCache<T, TKey>? distributed = null,
            DistributedCacheOptions<T, TKey, bool>? distributedCacheOptions = null) :
            base(query, cache, cacheOptions, distributed, distributedCacheOptions)
        {
        }
    }
}