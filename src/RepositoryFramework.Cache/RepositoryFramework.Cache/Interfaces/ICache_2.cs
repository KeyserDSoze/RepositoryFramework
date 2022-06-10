namespace RepositoryFramework.Cache
{
    public interface ICache<T, TKey> : ICache<T, TKey, bool>
        where TKey : notnull
    {
    }
}