namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T, TKey> : CachedRepository<T, TKey, State<T>>, IRepository<T, TKey>, ICommand<T, TKey>
         where TKey : notnull
    {
        public CachedRepository(IRepositoryPattern<T, TKey>? repository = null,
            ICommandPattern<T, TKey>? command = null,
            IQueryPattern<T, TKey>? query = null,
            ICache<T, TKey>? cache = null,
            CacheOptions<T, TKey, State<T>>? cacheOptions = null,
            IDistributedCache<T, TKey>? distributed = null,
            DistributedCacheOptions<T, TKey, State<T>>? distributedCacheOptions = null) :
            base(repository, command, query, cache, cacheOptions, distributed, distributedCacheOptions)
        {
        }
    }
}