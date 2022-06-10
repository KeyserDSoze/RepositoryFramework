namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T, TKey, TState> : CachedQuery<T, TKey, TState>, IRepository<T, TKey, TState>, IRepositoryPattern<T, TKey, TState>, ICommandPattern<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IRepositoryPattern, ICommandPattern, IQueryPattern
         where TKey : notnull
    {
        private readonly IRepositoryPattern<T, TKey, TState> _repository;

        public CachedRepository(IRepositoryPattern<T, TKey, TState> repository,
            ICache<T, TKey, TState>? cache = null,
            CacheOptions<T, TKey, TState>? cacheOptions = null,
            IDistributedCache<T, TKey, TState>? distributed = null,
            DistributedCacheOptions<T, TKey, TState>? distributedCacheOptions = null) :
            base(repository, cache, cacheOptions, distributed, distributedCacheOptions)
        {
            _repository = repository;
        }
        public Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(key, cancellationToken);

        public Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.InsertAsync(key, value, cancellationToken);

        public Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(key, value, cancellationToken);
    }
}