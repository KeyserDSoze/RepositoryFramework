using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public interface IApiBuilder<T, TKey>
        where TKey : notnull
    {
        IServiceCollection Services { get; }
        IRepositoryBuilder<T, TKey> Builder { get; }
        IApiBuilder<T, TKey> WithName(string name);
        IApiBuilder<T, TKey> WithStartingPath(string path);
        IApiBuilder<T, TKey> WithVersion(string version);
        IHttpClientBuilder<T, TKey> WithHttpClient(Action<HttpClient> configuredClient);
        IHttpClientBuilder<T, TKey> WithHttpClient(string domain);
    }
}
