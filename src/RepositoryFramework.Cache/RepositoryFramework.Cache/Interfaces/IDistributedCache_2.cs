namespace RepositoryFramework.Cache
{
    public interface IDistributedCache<T, TKey> : IDistributedCache<T, TKey, bool>, ICache<T, TKey>
        where TKey : notnull
    {
    }
}