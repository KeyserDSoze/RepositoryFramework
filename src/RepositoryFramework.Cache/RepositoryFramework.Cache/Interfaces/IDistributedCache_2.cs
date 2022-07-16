namespace RepositoryFramework.Cache
{
    public interface IDistributedCache<T, TKey> : IDistributedCache<T, TKey, State>, ICache<T, TKey>
        where TKey : notnull
    {
    }
}