namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T> : CachedRepository<T, string>, IRepository<T>
    {
        public CachedRepository(IRepositoryPattern<T>? repository = null,
            ICommandPattern<T>? command = null,
            IQueryPattern<T>? query = null,
            ICache<T>? cache = null,
            CacheOptions<T, string, State>? cacheOptions = null,
            IDistributedCache<T>? distributed = null,
            DistributedCacheOptions<T, string, State>? distributedCacheOptions = null) :
            base(repository, command, query, cache, cacheOptions, distributed, distributedCacheOptions)
        {
        }
    }
}