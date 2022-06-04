namespace RepositoryFramework.Client
{
    /// <summary>
    /// Interface for specific interceptor request for your repository or CQRS client of <typeparamref name="T"/> and <typeparamref name="TKey"/>.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface IRepositoryClientInterceptor<T> : IRepositoryClientInterceptor
    {
    }
}