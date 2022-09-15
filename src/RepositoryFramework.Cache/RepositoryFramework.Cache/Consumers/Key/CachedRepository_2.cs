namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T, TKey> : CachedQuery<T, TKey>, IRepository<T, TKey>, ICommand<T, TKey>
         where TKey : notnull
    {
        private readonly IRepositoryPattern<T, TKey>? _repository;
        private readonly ICommandPattern<T, TKey>? _command;

        public CachedRepository(IRepositoryPattern<T, TKey>? repository = null,
            ICommandPattern<T, TKey>? command = null,
            IQueryPattern<T, TKey>? query = null,
            ICache<T, TKey>? cache = null,
            CacheOptions<T, TKey>? cacheOptions = null,
            IDistributedCache<T, TKey>? distributed = null,
            DistributedCacheOptions<T, TKey>? distributedCacheOptions = null) :
            base(repository ?? query!, cache, cacheOptions, distributed, distributedCacheOptions)
        {
            _repository = repository;
            _command = command;
        }

        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            var results = await (_repository ?? _command!).BatchAsync(operations, cancellationToken).NoContext();
            foreach (var result in results.Results)
            {
                if (result.State.IsOk)
                {
                    RepositoryMethods method = (RepositoryMethods)(int)(result.Command);
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
                                _cacheOptions.HasCache(RepositoryMethods.Delete),
                                _distributedCacheOptions.HasCache(RepositoryMethods.Delete), cancellationToken).NoContext();
                        }
                    }
                }
            }
            return results;
        }

        public async Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethods.Delete))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethods.Delete)))
                await RemoveExistAndGetCacheAsync(key,
                    _cacheOptions.HasCache(RepositoryMethods.Delete),
                    _distributedCacheOptions.HasCache(RepositoryMethods.Delete), cancellationToken).NoContext();
            return await (_repository ?? _command!).DeleteAsync(key, cancellationToken).NoContext();
        }

        public async Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            State<T> result = await (_repository ?? _command!).InsertAsync(key, value, cancellationToken).NoContext();
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethods.Insert))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethods.Insert)))
                await UpdateExistAndGetCacheAsync(key, value,
                    await (_query ?? _repository!).ExistAsync(key, cancellationToken).NoContext(),
                    _cacheOptions.HasCache(RepositoryMethods.Insert),
                    _distributedCacheOptions.HasCache(RepositoryMethods.Insert),
                    cancellationToken).NoContext();
            return result;
        }

        public async Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            State<T> result = await (_repository ?? _command!).UpdateAsync(key, value, cancellationToken).NoContext();
            if ((_cache != null && _cacheOptions.HasCache(RepositoryMethods.Update))
                || (_distributed != null && _distributedCacheOptions.HasCache(RepositoryMethods.Update)))
                await UpdateExistAndGetCacheAsync(key, value,
                    await (_query ?? _repository!).ExistAsync(key, cancellationToken).NoContext(),
                    _cacheOptions.HasCache(RepositoryMethods.Update),
                    _distributedCacheOptions.HasCache(RepositoryMethods.Update),
                    cancellationToken).NoContext();
            return result;
        }
    }
}