using System.Text.Json;

namespace RepositoryFramework.Cache.Azure.Storage.Blob
{
    public class BlobStorageCache<T, TKey, TState> : IDistributedCache<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly IRepository<BlobStorageCacheModel, string> _repository;

        public BlobStorageCache(IRepository<BlobStorageCacheModel, string> repository)
        {
            _repository = repository;
        }

        public async Task<CacheResponse<TValue>> RetrieveAsync<TValue>(string key, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExistAsync(key, cancellationToken).NoContext())
            {
                var result = await _repository.GetAsync(key, cancellationToken).NoContext();
                if (DateTime.UtcNow < (result?.Expiration ?? DateTime.MaxValue))
                    return new(true, result?.Value != null ? JsonSerializer.Deserialize<TValue>(result.Value)! : default!);
            }
            return new(false, default);
        }

        public async Task<bool> SetAsync<TValue>(string key, TValue value, CacheOptions<T, TKey, TState> options, CancellationToken? cancellationToken = null)
            => (await _repository.UpdateAsync(key, new BlobStorageCacheModel
            {
                Expiration = DateTime.UtcNow.Add(options.ExpiringTime),
                Value = JsonSerializer.Serialize(value),
            }).NoContext()).IsOk;
        public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExistAsync(key, cancellationToken).NoContext())
                return await _repository.DeleteAsync(key, cancellationToken).NoContext();
            return true;
        }
    }
}
