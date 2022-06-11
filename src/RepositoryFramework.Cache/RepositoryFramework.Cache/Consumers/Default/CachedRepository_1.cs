namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T> : CachedRepository<T, string>, IRepository<T>, IRepositoryPattern<T>, ICommandPattern<T>, IQueryPattern<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
        public CachedRepository(IRepositoryPattern<T>? repository = null,
            ICommandPattern<T>? command = null,
            IQueryPattern<T>? query = null,
            ICache<T>? cache = null,
            CacheOptions<T, string, bool>? cacheOptions = null,
            IDistributedCache<T>? distributed = null,
            DistributedCacheOptions<T, string, bool>? distributedCacheOptions = null) :
            base(repository, command, query, cache, cacheOptions, distributed, distributedCacheOptions)
        {
        }
    }
}