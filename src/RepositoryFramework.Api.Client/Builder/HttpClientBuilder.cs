using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    internal sealed class HttpClientBuilder<T, TKey> : IHttpClientBuilder<T, TKey>
        where TKey : notnull
    {
        public IApiBuilder<T, TKey> Builder { get; }
        public IHttpClientBuilder ClientBuilder { get; }
        public HttpClientBuilder(IApiBuilder<T, TKey> apiBuilder, IHttpClientBuilder clientBuilder)
        {
            Builder = apiBuilder;
            ClientBuilder = clientBuilder;
        }
    }
}
