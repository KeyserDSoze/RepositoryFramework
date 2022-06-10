namespace RepositoryFramework.Cache
{
    public class CacheOptions<T, TKey, TState>
        where TKey : notnull
    {
        public TimeSpan RefreshTime { get; set; }
        public static CacheOptions<T, TKey, TState> Default { get; } = new CacheOptions<T, TKey, TState>()
        {
            RefreshTime = TimeSpan.FromDays(365 * 365)
        };
    }
}
