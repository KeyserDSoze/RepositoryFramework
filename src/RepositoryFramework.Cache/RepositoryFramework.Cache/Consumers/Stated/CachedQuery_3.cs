using System.Linq.Expressions;

namespace RepositoryFramework.Cache
{
    internal class CachedQuery<T, TKey, TState> : IQuery<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IQueryPattern
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
                toDelete.Add(_cache.DeleteAsync(getKeyAsString, cancellationToken));
                toDelete.Add(_cache.DeleteAsync(existKeyAsString, cancellationToken));
            }
            if (inDistributed && _distributed != null)
            {
                toDelete.Add(_distributed.DeleteAsync(getKeyAsString, cancellationToken));
                toDelete.Add(_distributed.DeleteAsync(existKeyAsString, cancellationToken));
            }
            return Task.WhenAll(toDelete);
        }
        private protected Task UpdateExistAndGetCacheAsync(TKey key, T value, TState state, bool inMemory, bool inDistributed, CancellationToken cancellationToken = default)
        {
            string existKeyAsString = $"{nameof(RepositoryMethod.Exist)}_{key}";
            string getKeyAsString = $"{nameof(RepositoryMethod.Get)}_{key}";
            List<Task> toDelete = new();
            if (_cache != null || _distributed != null)
            {
                toDelete.Add(SaveOnCacheAsync(getKeyAsString, value, Source.Repository, inMemory, inDistributed, cancellationToken));
                toDelete.Add(SaveOnCacheAsync(existKeyAsString, state, Source.Repository, inMemory, inDistributed, cancellationToken));
            }
            return Task.WhenAll(toDelete);
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

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"{nameof(RepositoryMethod.Query)}_{predicate}_{top}_{skip}";

            var value = await RetrieveValueAsync<IEnumerable<T>>(RepositoryMethod.Query, keyAsString,
                () => _query.QueryAsync(predicate, top, skip, cancellationToken)!, cancellationToken);

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