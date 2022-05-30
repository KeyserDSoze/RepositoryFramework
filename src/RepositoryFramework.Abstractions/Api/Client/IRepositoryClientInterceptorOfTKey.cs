namespace RepositoryFramework.Client
{
    public interface IRepositoryClientInterceptor<T, TKey> : IRepositoryClientInterceptor
        where TKey : notnull
    {
    }
}