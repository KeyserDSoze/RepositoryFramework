using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public interface IRepositoryHttpClientBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryApiBuilder<T, TKey> ApiBuilder { get; }
        IHttpClientBuilder ClientBuilder { get; }
        IRepositoryBuilder<T, TKey> RepositoryBuilder { get; }
        IRepositoryHttpClientBuilder<T, TKey> WithDefaultRetryPolicy();
    }
}
