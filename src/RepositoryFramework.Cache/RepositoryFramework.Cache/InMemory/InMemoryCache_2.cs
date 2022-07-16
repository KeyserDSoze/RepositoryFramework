namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T, TKey> : InMemoryCache<T, TKey, State>, ICache<T, TKey>
        where TKey : notnull
    {
       
    }
}