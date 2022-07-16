﻿using System.Text.Json;

namespace RepositoryFramework.Cache.Azure.Storage.Blob
{
    public class BlobStorageCache<T, TKey, TState> : IDistributedCache<T, TKey, TState>
        where TKey : notnull
        where TState : IState
    {
        private readonly IRepository<BlobStorageCacheModel, string> _repository;

        public BlobStorageCache(IRepository<BlobStorageCacheModel, string> repository)
        {
            _repository = repository;
        }

        public async Task<(bool IsPresent, TValue Response)> RetrieveAsync<TValue>(string key, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExistAsync(key, cancellationToken))
            {
                var result = await _repository.GetAsync(key, cancellationToken);
                if (DateTime.UtcNow < (result?.Expiration ?? DateTime.MaxValue))
                    return (true, result?.Value != null ? JsonSerializer.Deserialize<TValue>(result.Value)! : default!);
            }
            return (false, default(TValue)!);
        }

        public async Task<bool> SetAsync<TValue>(string key, TValue value, CacheOptions<T, TKey, TState> options, CancellationToken? cancellationToken = null)
            => (await _repository.UpdateAsync(key, new BlobStorageCacheModel
            {
                Expiration = DateTime.UtcNow.Add(options.RefreshTime),
                Value = JsonSerializer.Serialize(value),
            })).IsOk;
        public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExistAsync(key, cancellationToken))
                return await _repository.DeleteAsync(key, cancellationToken);
            return true;
        }
    }
}
