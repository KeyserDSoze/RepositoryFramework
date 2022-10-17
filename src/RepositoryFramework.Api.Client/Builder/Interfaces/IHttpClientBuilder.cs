using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public interface IHttpClientBuilder<T, TKey>
        where TKey : notnull
    {
        IApiBuilder<T, TKey> Builder { get; }
        IHttpClientBuilder ClientBuilder { get; }
        IHttpClientBuilder<T, TKey> WithDefaultRetryPolicy();
    }
}
