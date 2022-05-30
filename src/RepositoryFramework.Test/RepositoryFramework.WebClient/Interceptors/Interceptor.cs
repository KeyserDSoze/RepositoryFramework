using RepositoryFramework.Client;
using RepositoryFramework.WebClient.Data;

namespace RepositoryFramework.WebClient.Interceptors
{
    public class Interceptor : IRepositoryClientInterceptor
    {
        public Task<HttpClient> EnrichAsync(HttpClient client, ApiName path)
            => Task.FromResult(client);
    }
    public class SpecificInterceptor : IRepositoryClientInterceptor<User, string>
    {
        public Task<HttpClient> EnrichAsync(HttpClient client, ApiName path) 
            => Task.FromResult(client);
    }
}
