using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace RepositoryFramework.Cache
{
    internal class DistributedCache<T, TKey, TState> : IDistributedCache<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCache(IDistributedCache distributedCache)
            => _distributedCache = distributedCache;

        public async Task<CacheResponse<TValue>> RetrieveAsync<TValue>(string key, CancellationToken cancellationToken = default)
        {
            var response = await _distributedCache.GetAsync(key, cancellationToken).NoContext();
            if (response == null || response.Length == 0)
                return new(false, default!);
            return new(true, JsonSerializer.Deserialize<TValue>(Encoding.UTF8.GetString(response))!);
        }

        public async Task<bool> SetAsync<TValue>(string key, TValue value, CacheOptions<T, TKey, TState> options, CancellationToken? cancellationToken = null)
        {
            await _distributedCache.SetAsync(key,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.Add(options.ExpiringTime),
                    SlidingExpiration = options.ExpiringTime,
                    AbsoluteExpirationRelativeToNow = options.ExpiringTime,
                }).NoContext();
            return true;
        }

        public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken).NoContext();
            return true;
        }
    }
}