namespace RepositoryFramework.Cache
{
    public interface ICache<T, TKey, TState>
        where TKey : notnull
    {
        Task<(bool IsPresent, TValue Response)> RetrieveAsync<TValue>(string key, CancellationToken cancellationToken = default);
        Task<bool> SetAsync<TValue>(string key, TValue value, CacheOptions<T, TKey, TState> options, CancellationToken? cancellationToken = null);
        Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default);
    }
}