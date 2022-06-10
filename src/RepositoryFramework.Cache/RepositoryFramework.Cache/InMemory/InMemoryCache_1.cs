namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T> : InMemoryCache<T, string>, ICache<T>
    {
    }
}