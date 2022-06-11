namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T, TKey, TState> : CachedQuery<T, TKey, TState>, IRepository<T, TKey, TState>, ICommand<T, TKey, TState>, IRepositoryPattern<T, TKey, TState>, ICommandPattern<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IRepositoryPattern, ICommandPattern, IQueryPattern
         where TKey : notnull
    {
        private readonly IRepositoryPattern<T, TKey, TState>? _repository;
        private readonly ICommandPattern<T, TKey, TState>? _command;

        public CachedRepository(IRepositoryPattern<T, TKey, TState>? repository = null,
            ICommandPattern<T, TKey, TState>? command = null,
            IQueryPattern<T, TKey, TState>? query = null,
            ICache<T, TKey, TState>? cache = null,
            CacheOptions<T, TKey, TState>? cacheOptions = null,
            IDistributedCache<T, TKey, TState>? distributed = null,
            DistributedCacheOptions<T, TKey, TState>? distributedCacheOptions = null) :
            base(repository ?? query!, cache, cacheOptions, distributed, distributedCacheOptions)
        {
            _repository = repository;
            _command = command;
        }
        public async Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethod.Delete))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethod.Delete)))
                await RemoveExistAndGetCacheAsync(key, 
                    _cacheOptions.HasCache(RepositoryMethod.Delete),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Delete), cancellationToken);
            return await (_repository ?? _command!).DeleteAsync(key, cancellationToken);
        }

        public async Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            TState result = await (_repository ?? _command!).InsertAsync(key, value, cancellationToken);
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethod.Insert))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethod.Insert)))
                await UpdateExistAndGetCacheAsync(key, value, await (_query ?? _repository!).ExistAsync(key, cancellationToken),
                    _cacheOptions.HasCache(RepositoryMethod.Insert),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Insert),
                    cancellationToken);
            return result;
        }

        public async Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            TState result = await (_repository ?? _command!).UpdateAsync(key, value, cancellationToken);
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethod.Update))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethod.Update)))
                await UpdateExistAndGetCacheAsync(key, value, await (_query ?? _repository!).ExistAsync(key, cancellationToken),
                    _cacheOptions.HasCache(RepositoryMethod.Update),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Update),
                    cancellationToken);
            return result;
        }
    }
}