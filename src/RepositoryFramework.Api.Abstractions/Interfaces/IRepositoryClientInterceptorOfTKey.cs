namespace RepositoryFramework.Client
{
    /// <summary>
    /// Interface for specific interceptor request for your repository or CQRS client of <typeparamref name="T"/> and <typeparamref name="TKey"/>.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository</typeparam>
    public interface IRepositoryClientInterceptor<T, TKey> : IRepositoryClientInterceptor
        where TKey : notnull
    {
    }
    /// <summary>
    /// Interface for specific interceptor request for your repository or CQRS client of <typeparamref name="T"/> and <typeparamref name="TKey"/>.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface IRepositoryClientInterceptor<T> : IRepositoryClientInterceptor
    {
    }
}