namespace RepositoryFramework.Client
{
    public interface IRepositoryClientInterceptor
    {
        Task<HttpClient> EnrichAsync(HttpClient client, ApiName path);
    }
}