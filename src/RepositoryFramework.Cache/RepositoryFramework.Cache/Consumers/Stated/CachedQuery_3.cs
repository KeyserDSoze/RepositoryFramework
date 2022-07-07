using System.Linq.Expressions;

namespace RepositoryFramework.Cache
{
    internal class CachedQuery<T, TKey, TState> : IQuery<T, TKey, TState>
         where TKey : notnull
    {
        private protected readonly IQueryPattern<T, TKey, TState> _query;
        private protected readonly ICache<T, TKey, TState>? _cache;
        private protected readonly CacheOptions<T, TKey, TState> _cacheOptions;
        private protected readonly IDistributedCache<T, TKey, TState>? _distributed;
        private protected readonly DistributedCacheOptions<T, TKey, TState> _distributedCacheOptions;

        public CachedQuery(IQueryPattern<T, TKey, TState> query,
            ICache<T, TKey, TState>? cache = null,
            CacheOptions<T, TKey, TState>? cacheOptions = null,
            IDistributedCache<T, TKey, TState>? distributed = null,
            DistributedCacheOptions<T, TKey, TState>? distributedCacheOptions = null)
        {
            _query = query;
            _cache = cache;
            _cacheOptions = cacheOptions ?? CacheOptions<T, TKey, TState>.Default;
            _distributed = distributed;
            _distributedCacheOptions = distributedCacheOptions ?? DistributedCacheOptions<T, TKey, TState>.Default;
        }
        private protected Task RemoveExistAndGetCacheAsync(TKey key, bool inMemory, bool inDistributed, CancellationToken cancellationToken = default)
        {
            string existKeyAsString = $"{nameof(RepositoryMethod.Exist)}_{key}";
            string getKeyAsString = $"{nameof(RepositoryMethod.Get)}_{key}";
            List<Task> toDelete = new();
            if (inMemory && _cache != null)
            {
                if (_cacheOptions.HasCache(RepositoryMethod.Get))
                    toDelete.Add(_cache.DeleteAsync(getKeyAsString, cancellationToken));
                if (_cacheOptions.HasCache(RepositoryMethod.Exist))
                    toDelete.Add(_cache.DeleteAsync(existKeyAsString, cancellationToken));
            }
            if (inDistributed && _distributed != null)
            {
                if (_distributedCacheOptions.HasCache(RepositoryMethod.Get))
                    toDelete.Add(_distributed.DeleteAsync(getKeyAsString, cancellationToken));
                if (_distributedCacheOptions.HasCache(RepositoryMethod.Exist))
                    toDelete.Add(_distributed.DeleteAsync(existKeyAsString, cancellationToken));
            }
            return Task.WhenAll(toDelete);
        }
        private protected Task UpdateExistAndGetCacheAsync(TKey key, T value, TState state, bool inMemory, bool inDistributed, CancellationToken cancellationToken = default)
        {
            string existKeyAsString = $"{nameof(RepositoryMethod.Exist)}_{key}";
            string getKeyAsString = $"{nameof(RepositoryMethod.Get)}_{key}";
            List<Task> toUpdate = new();
            if (_cache != null || _distributed != null)
            {
                toUpdate.Add(SaveOnCacheAsync(getKeyAsString, value, Source.Repository,
                    inMemory && _cacheOptions?.HasCache(RepositoryMethod.Get) == true,
                    inDistributed && _distributedCacheOptions?.HasCache(RepositoryMethod.Get) == true,
                    cancellationToken));
                toUpdate.Add(SaveOnCacheAsync(existKeyAsString, state, Source.Repository,
                    inMemory && _cacheOptions?.HasCache(RepositoryMethod.Exist) == true,
                    inDistributed && _distributedCacheOptions?.HasCache(RepositoryMethod.Exist) == true,
                    cancellationToken));
            }
            return Task.WhenAll(toUpdate);
        }
        public async Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"{nameof(RepositoryMethod.Exist)}_{key}";
            var value = await RetrieveValueAsync(RepositoryMethod.Exist, keyAsString,
                () => _query.ExistAsync(key, cancellationToken)!, cancellationToken);

            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, value.Response, value.Source,
                    _cacheOptions.HasCache(RepositoryMethod.Exist),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Exist),
                    cancellationToken);

            return value.Response!;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"{nameof(RepositoryMethod.Get)}_{key}";
            var value = await RetrieveValueAsync<T?>(RepositoryMethod.Get, keyAsString,
                () => _query.GetAsync(key, cancellationToken), cancellationToken);

            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, value.Response, value.Source,
                    _cacheOptions.HasCache(RepositoryMethod.Get),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Get),
                    cancellationToken);

            return value.Response;
        }

        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"{nameof(RepositoryMethod.Query)}_{options?.Predicate}_{options?.Top}_{options?.Skip}_{options?.Order}_{options?.IsAscending}";

            var value = await RetrieveValueAsync<IEnumerable<T>>(RepositoryMethod.Query, keyAsString,
                () => _query.QueryAsync(options, cancellationToken)!, cancellationToken);

            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, value.Response, value.Source,
                    _cacheOptions.HasCache(RepositoryMethod.Query),
                    _distributedCacheOptions.HasCache(RepositoryMethod.Query),
                    cancellationToken);

            return value.Response ?? new List<T>();
        }

        private Task SaveOnCacheAsync<TResponse>(string key, TResponse response, Source source, bool inMemory, bool inDistributed, CancellationToken cancellationToken)
        {
            List<Task> cacheSaverTasks = new();
            if (inMemory && _cache != null && source > Source.InMemory)
                cacheSaverTasks.Add(_cache.SetAsync(key, response, _cacheOptions, cancellationToken));
            if (inDistributed && _distributed != null && source > Source.Distributed)
                cacheSaverTasks.Add(_distributed.SetAsync(key, response, _distributedCacheOptions, cancellationToken));
            return Task.WhenAll(cacheSaverTasks);
        }
        private async Task<(Source Source, TValue? Response)> RetrieveValueAsync<TValue>(RepositoryMethod method, string key, Func<Task<TValue?>> action, CancellationToken cancellationToken)
        {
            if (_cache != null && _cacheOptions.HasCache(method))
            {
                var (IsPresent, Response) = await _cache.RetrieveAsync<TValue>(key, cancellationToken);
                if (IsPresent)
                    return (Source.InMemory, Response);
            }
            if (_distributed != null && _distributedCacheOptions.HasCache(method))
            {
                var (IsPresent, Response) = await _distributed.RetrieveAsync<TValue>(key, cancellationToken);
                if (IsPresent)
                    return (Source.Distributed, Response);
            }
            return (Source.Repository, await action.Invoke());
        }
        private enum Source
        {
            InMemory,
            Distributed,
            Repository
        }
    }
}