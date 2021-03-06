namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T, TKey, TState> : CachedQuery<T, TKey, TState>, IRepository<T, TKey, TState>, ICommand<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
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

        public async Task<BatchResults<TKey, TState>> BatchAsync(BatchOperations<T, TKey, TState> operations, CancellationToken cancellationToken = default)
        {
            var results = await (_repository ?? _command!).BatchAsync(operations, cancellationToken).NoContext();
            foreach (var result in results.Results)
            {
                if (result.State.IsOk)
                {
                    RepositoryMethod method = (RepositoryMethod)(int)(result.Command);
                    if ((_cache != null && _cacheOptions.HasCache(method))
                        || (_distributed != null && _distributedCacheOptions.HasCache(method)))
                    {
                        var operation = operations.Values.First(x => x.Key.Equals(result.Key));
                        if (result.Command != CommandType.Delete)
                        {
                            await UpdateExistAndGetCacheAsync(operation.Key, operation.Value!,
                                await (_query ?? _repository!).ExistAsync(operation.Key, cancellationToken).NoContext(),
                                _cacheOptions.HasCache(method),
                                _distributedCacheOptions.HasCache(method),
                                cancellationToken).NoContext();
                        }
                        else
                        {
                            await RemoveExistAndGetCacheAsync(operation.Key,
                                _cacheOptions.HasCache(RepositoryMethod.Delete),
                                _distributedCacheOptions.HasCache(RepositoryMethod.Delete), cancellationToken).NoContext();
                        }
                    }
                }
            }
            return results;
        }

        public async Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethod.Delete))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethod.Delete)))
                await RemoveExistAndGetCacheAsync(key,
                    _cacheOptions.HasCache(RepositoryMethod.Delete),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Delete), cancellationToken).NoContext();
            return await (_repository ?? _command!).DeleteAsync(key, cancellationToken).NoContext();
        }

        public async Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            TState result = await (_repository ?? _command!).InsertAsync(key, value, cancellationToken).NoContext();
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethod.Insert))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethod.Insert)))
                await UpdateExistAndGetCacheAsync(key, value, 
                    await (_query ?? _repository!).ExistAsync(key, cancellationToken).NoContext(),
                    _cacheOptions.HasCache(RepositoryMethod.Insert),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Insert),
                    cancellationToken).NoContext();
            return result;
        }

        public async Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            TState result = await (_repository ?? _command!).UpdateAsync(key, value, cancellationToken).NoContext();
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethod.Update))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethod.Update)))
                await UpdateExistAndGetCacheAsync(key, value,
                    await (_query ?? _repository!).ExistAsync(key, cancellationToken).NoContext(),
                    _cacheOptions.HasCache(RepositoryMethod.Update),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Update),
                    cancellationToken).NoContext();
            return result;
        }
    }
}