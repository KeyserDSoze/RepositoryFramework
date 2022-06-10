using System.Collections.Concurrent;

namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T, TKey, TState> : ICache<T, TKey, TState>
        where TKey : notnull
    {
        private class InMemoryCacheValue
        {
            public object? Value { get; set; }
            public DateTime ExpiringTime { get; set; }
        }
        private readonly ConcurrentDictionary<string, InMemoryCacheValue> _cache = new();
        public Task<(bool IsPresent, TValue Response)> RetrieveAsync<TValue>(string key, CancellationToken cancellationToken = default)
        {
            if (_cache.ContainsKey(key))
            {
                var cached = _cache[key];
                if (cached.ExpiringTime < DateTime.UtcNow)
                    return Task.FromResult((true, cached.Value == null ? default! : (TValue)cached.Value));
            }
            return Task.FromResult((false, default(TValue)!));
        }

        public Task<bool> SetAsync<TValue>(string key, TValue value, CacheOptions<T, TKey, TState> options, CancellationToken? cancellationToken = null)
        {
            var cached = new InMemoryCacheValue
            {
                Value = value,
                ExpiringTime = DateTime.UtcNow.Add(options.RefreshTime)
            };
            if (!_cache.TryAdd(key, cached))
                _cache[key] = cached;
            return Task.FromResult(true);
        }
    }
}