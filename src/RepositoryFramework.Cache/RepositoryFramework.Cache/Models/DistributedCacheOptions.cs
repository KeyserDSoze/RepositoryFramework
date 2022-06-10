namespace RepositoryFramework.Cache
{
    public class DistributedCacheOptions<T, TKey, TState> : CacheOptions<T, TKey, TState>
        where TKey : notnull
    {
        public static new DistributedCacheOptions<T, TKey, TState> Default { get; } =
            new DistributedCacheOptions<T, TKey, TState>()
            {
                RefreshTime = TimeSpan.FromDays(365 * 365)
            };
    }
}
