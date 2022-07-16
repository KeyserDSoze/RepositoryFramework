namespace RepositoryFramework.Cache
{
    internal class CachedQuery<T, TKey> : CachedQuery<T, TKey, State>, IQuery<T, TKey>
         where TKey : notnull
    {
        public CachedQuery(IQueryPattern<T, TKey> query,
            ICache<T, TKey>? cache = null,
            CacheOptions<T, TKey, State>? cacheOptions = null,
            IDistributedCache<T, TKey>? distributed = null,
            DistributedCacheOptions<T, TKey, State>? distributedCacheOptions = null) :
            base(query, cache, cacheOptions, distributed, distributedCacheOptions)
        {
        }
    }
}