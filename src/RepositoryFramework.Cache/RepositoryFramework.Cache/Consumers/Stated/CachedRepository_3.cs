namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T, TKey, TState> : CachedQuery<T, TKey, TState>, IRepository<T, TKey, TState>, ICommand<T, TKey, TState>
        where TKey : notnull
        where TState : IState
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

        public async Task<IEnumerable<BatchResult<TKey, TState>>> BatchAsync(IEnumerable<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default)
        {
            var results = await (_repository ?? _command!).BatchAsync(operations, cancellationToken);
            foreach (var result in results)
            {
                if (result.State.IsOk)
                {
                    RepositoryMethod method = (RepositoryMethod)(int)(result.Command);
                    if ((_cache != null && _cacheOptions.HasCache(method))
                        || (_distributed != null && _distributedCacheOptions.HasCache(method)))
                    {
                        var operation = operations.First(x => x.Key.Equals(result.Key));
                        if (result.Command != CommandType.Delete)
                        {
                            await UpdateExistAndGetCacheAsync(operation.Key, operation.Value!,
                                await (_query ?? _repository!).ExistAsync(operation.Key, cancellationToken),
                                _cacheOptions.HasCache(method),
                                _distributedCacheOptions.HasCache(method),
                                cancellationToken);
                        }
                        else
                        {
                            await RemoveExistAndGetCacheAsync(operation.Key,
                                _cacheOptions.HasCache(RepositoryMethod.Delete),
                                _distributedCacheOptions.HasCache(RepositoryMethod.Delete), cancellationToken);
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