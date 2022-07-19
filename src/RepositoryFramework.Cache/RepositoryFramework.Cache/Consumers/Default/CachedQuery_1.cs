namespace RepositoryFramework.Cache
{
    internal sealed class CachedQuery<T> : CachedQuery<T, string>, IQuery<T>
    {
        public CachedQuery(IQueryPattern<T> query,
            ICache<T>? cache = null,
            CacheOptions<T, string, State<T>>? cacheOptions = null,
            IDistributedCache<T>? distributed = null,
            DistributedCacheOptions<T, string, State<T>>? distributedCacheOptions = null) :
            base(query, cache, cacheOptions, distributed, distributedCacheOptions)
        {
        }
    }
}