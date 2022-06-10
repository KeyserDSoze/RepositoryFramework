namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T, TKey> : InMemoryCache<T, TKey, bool>, ICache<T, TKey>
        where TKey : notnull
    {
       
    }
}