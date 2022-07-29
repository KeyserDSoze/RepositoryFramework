using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T, TKey, TState> : ICache<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly IMemoryCache _memoryCache;
        public InMemoryCache(IMemoryCache memoryCache)
            => _memoryCache = memoryCache;
        public Task<CacheResponse<TValue>> RetrieveAsync<TValue>(string key, CancellationToken cancellationToken = default)
        {
            bool isPresent = _memoryCache.TryGetValue(key, out TValue value);
            return Task.FromResult(new CacheResponse<TValue>(isPresent, value));
        }

        public Task<bool> SetAsync<TValue>(string key, TValue value, CacheOptions<T, TKey, TState> options, CancellationToken? cancellationToken = null)
        {
            _memoryCache.Set(key, value, options.ExpiringTime);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            _memoryCache.Remove(key);
            return Task.FromResult(true);
        }
    }
}