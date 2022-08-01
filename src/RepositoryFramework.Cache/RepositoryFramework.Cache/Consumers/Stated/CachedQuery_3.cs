using System.Linq.Expressions;

namespace RepositoryFramework.Cache
{
    internal class CachedQuery<T, TKey, TState> : IQuery<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private protected readonly IQueryPattern<T, TKey, TState> _query;
        private protected readonly ICache<T, TKey, TState>? _cache;
        private protected readonly CacheOptions<T, TKey, TState> _cacheOptions;
        private protected readonly IDistributedCache<T, TKey, TState>? _distributed;
        private protected readonly DistributedCacheOptions<T, TKey, TState> _distributedCacheOptions;
        private readonly string _cacheName;

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
            _cacheName = typeof(T).Name;
        }
        private protected Task RemoveExistAndGetCacheAsync(TKey key, bool inMemory, bool inDistributed, CancellationToken cancellationToken = default)
        {
            string existKeyAsString = key.AsStringWithPrefix($"{nameof(RepositoryMethods.Exist)}_{_cacheName}");
            string getKeyAsString = key.AsStringWithPrefix($"{nameof(RepositoryMethods.Get)}_{_cacheName}");
            List<Task> toDelete = new();
            if (inMemory && _cache != null)
            {
                if (_cacheOptions.HasCache(RepositoryMethods.Get))
                    toDelete.Add(_cache.DeleteAsync(getKeyAsString, cancellationToken));
                if (_cacheOptions.HasCache(RepositoryMethods.Exist))
                    toDelete.Add(_cache.DeleteAsync(existKeyAsString, cancellationToken));
            }
            if (inDistributed && _distributed != null)
            {
                if (_distributedCacheOptions.HasCache(RepositoryMethods.Get))
                    toDelete.Add(_distributed.DeleteAsync(getKeyAsString, cancellationToken));
                if (_distributedCacheOptions.HasCache(RepositoryMethods.Exist))
                    toDelete.Add(_distributed.DeleteAsync(existKeyAsString, cancellationToken));
            }
            return Task.WhenAll(toDelete);
        }
        private protected Task UpdateExistAndGetCacheAsync(TKey key, T value, TState state, bool inMemory, bool inDistributed, CancellationToken cancellationToken = default)
        {
            string existKeyAsString = key.AsStringWithPrefix($"{nameof(RepositoryMethods.Exist)}_{_cacheName}");
            string getKeyAsString = key.AsStringWithPrefix($"{nameof(RepositoryMethods.Get)}_{_cacheName}");
            List<Task> toUpdate = new();
            if (_cache != null || _distributed != null)
            {
                toUpdate.Add(SaveOnCacheAsync(getKeyAsString, value, Source.Repository,
                    inMemory && _cacheOptions?.HasCache(RepositoryMethods.Get) == true,
                    inDistributed && _distributedCacheOptions?.HasCache(RepositoryMethods.Get) == true,
                    cancellationToken));
                toUpdate.Add(SaveOnCacheAsync(existKeyAsString, state, Source.Repository,
                    inMemory && _cacheOptions?.HasCache(RepositoryMethods.Exist) == true,
                    inDistributed && _distributedCacheOptions?.HasCache(RepositoryMethods.Exist) == true,
                    cancellationToken));
            }
            return Task.WhenAll(toUpdate);
        }
        public async Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            string keyAsString = key.AsStringWithPrefix($"{nameof(RepositoryMethods.Exist)}_{_cacheName}");
            var value = await RetrieveValueAsync(RepositoryMethods.Exist, keyAsString,
                () => _query.ExistAsync(key, cancellationToken)!, cancellationToken).NoContext();

            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, value.Response, value.Source,
                    _cacheOptions.HasCache(RepositoryMethods.Exist),
                    _distributedCacheOptions.HasCache(RepositoryMethods.Exist),
                    cancellationToken).NoContext();

            return value.Response!;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            string keyAsString = key.AsStringWithPrefix($"{nameof(RepositoryMethods.Get)}_{_cacheName}");
            var value = await RetrieveValueAsync<T?>(RepositoryMethods.Get, keyAsString,
                () => _query.GetAsync(key, cancellationToken), cancellationToken).NoContext();

            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, value.Response, value.Source,
                    _cacheOptions.HasCache(RepositoryMethods.Get),
                    _distributedCacheOptions.HasCache(RepositoryMethods.Get),
                    cancellationToken).NoContext();

            return value.Response;
        }

        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"{nameof(RepositoryMethods.Query)}_{_cacheName}_{options?.Predicate}_{options?.Top}_{options?.Skip}_{options?.Order}_{options?.IsAscending}";

            var value = await RetrieveValueAsync(RepositoryMethods.Query, keyAsString,
                () => _query.QueryAsync(options, cancellationToken)!, cancellationToken).NoContext();

            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, value.Response.ToList(), value.Source,
                    _cacheOptions.HasCache(RepositoryMethods.Query),
                    _distributedCacheOptions.HasCache(RepositoryMethods.Query),
                    cancellationToken).NoContext();

            return value.Response ?? new List<T>();
        }
        public async Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"{nameof(RepositoryMethods.Count)}_{_cacheName}_{options?.Predicate}_{options?.Top}_{options?.Skip}_{options?.Order}_{options?.IsAscending}";

            var value = await RetrieveValueAsync(RepositoryMethods.Count, keyAsString,
                () => _query.CountAsync(options, cancellationToken)!, cancellationToken).NoContext();

            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, value.Response, value.Source,
                    _cacheOptions.HasCache(RepositoryMethods.Query),
                    _distributedCacheOptions.HasCache(RepositoryMethods.Query),
                    cancellationToken).NoContext();

            return value.Response;
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
        private async Task<(Source Source, TValue? Response)> RetrieveValueAsync<TValue>(RepositoryMethods method, string key, Func<Task<TValue?>> action, CancellationToken cancellationToken)
        {
            if (_cache != null && _cacheOptions.HasCache(method))
            {
                var (IsPresent, Response) = await _cache.RetrieveAsync<TValue>(key, cancellationToken).NoContext();
                if (IsPresent)
                    return (Source.InMemory, Response);
            }
            if (_distributed != null && _distributedCacheOptions.HasCache(method))
            {
                var (IsPresent, Response) = await _distributed.RetrieveAsync<TValue>(key, cancellationToken).NoContext();
                if (IsPresent)
                    return (Source.Distributed, Response);
            }
            return (Source.Repository, await action.Invoke().NoContext());
        }



        private enum Source
        {
            InMemory,
            Distributed,
            Repository
        }
    }
}