namespace RepositoryFramework.ApiClient
{
    /// <summary>
    /// Interface for global interceptor request for your repository or CQRS clients.
    /// </summary>
    public interface IRepositoryClientInterceptor
    {
        Task<HttpClient> EnrichAsync(HttpClient client, RepositoryMethod path);
    }
}