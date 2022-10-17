using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

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
        public IHttpClientBuilder<T, TKey> AddDefaultRetryPolicy()
        {
            var defaultPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrTransientHttpError()
                .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(5), 10, TimeSpan.FromSeconds(10));
            ClientBuilder
                .AddPolicyHandler(defaultPolicy);
            return this;
        }
    }
}
