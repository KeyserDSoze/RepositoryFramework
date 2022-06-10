using System.Linq.Expressions;

namespace RepositoryFramework.Cache
{
    internal class CachedQuery<T, TKey, TState> : IQuery<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IQueryPattern
         where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey, TState> _query;
        private readonly ICache<T, TKey, TState>? _cache;
        private readonly CacheOptions<T, TKey, TState> _cacheOptions;
        private readonly IDistributedCache<T, TKey, TState>? _distributed;
        private readonly DistributedCacheOptions<T, TKey, TState> _distributedCacheOptions;

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

        public async Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"exist_{key}";
            if (_cache != null || _distributed != null)
            {
                var (IsPresent, Response) = await GetFromCacheAsync<TState>(keyAsString, cancellationToken);
                if (IsPresent && Response != null)
                    return Response;
            }

            var response = await _query.ExistAsync(key, cancellationToken);
            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, response, cancellationToken);

            return response;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"get_{key}";
            if (_cache != null || _distributed != null)
            {
                var (IsPresent, Response) = await GetFromCacheAsync<T>(keyAsString, cancellationToken);
                if (IsPresent)
                    return Response;
            }

            var response = await _query.GetAsync(key, cancellationToken);
            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, response, cancellationToken);

            return response;
        }

        private static readonly IEnumerable<T> Empty = new List<T>();
        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            string keyAsString = $"query_{predicate}_{top}_{skip}";
            if (_cache != null || _distributed != null)
            {
                var (IsPresent, Response) = await GetFromCacheAsync<IEnumerable<T>>(keyAsString, cancellationToken);
                if (IsPresent)
                    return Response ?? Empty;
            }

            var response = await _query.QueryAsync(predicate, top, skip, cancellationToken);
            if (_cache != null || _distributed != null)
                await SaveOnCacheAsync(keyAsString, response, cancellationToken);

            return response ?? Empty;
        }

        private Task SaveOnCacheAsync<TResponse>(string key, TResponse response, CancellationToken cancellationToken)
        {
            List<Task> cacheSaverTasks = new();
            if (_cache != null && response != null)
                cacheSaverTasks.Add(_cache.SetAsync(key, response, _cacheOptions, cancellationToken));
            if (_distributed != null && response != null)
                cacheSaverTasks.Add(_distributed.SetAsync(key, response, _distributedCacheOptions, cancellationToken));
            return Task.WhenAll(cacheSaverTasks);
        }
        private async Task<(bool IsPresent, TValue? Response)> GetFromCacheAsync<TValue>(string key, CancellationToken cancellationToken)
        {
            if (_cache != null)
            {
                var (IsPresent, Response) = await _cache.RetrieveAsync<TValue>(key, cancellationToken);
                if (IsPresent)
                    return (true, Response);
            }
            if (_distributed != null)
            {
                var (IsPresent, Response) = await _distributed.RetrieveAsync<TValue>(key, cancellationToken);
                if (IsPresent)
                    return (true, Response);
            }
            return (false, default(TValue));
        }
    }
}