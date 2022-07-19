namespace RepositoryFramework.Cache
{
    public interface ICache<T, TKey> : ICache<T, TKey, State<T>>
        where TKey : notnull
    {
    }
}