namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T, TKey> : InMemoryCache<T, TKey, State<T>>, ICache<T, TKey>
        where TKey : notnull
    {
       
    }
}