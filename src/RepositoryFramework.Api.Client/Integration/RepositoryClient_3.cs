using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace RepositoryFramework.Api.Client
{
    internal class RepositoryClient<T, TKey, TState> : IRepositoryPattern<T, TKey, TState>
        where TKey : notnull
    {
        private readonly HttpClient _httpClient;
        private readonly IRepositoryClientInterceptor? _clientInterceptor;
        private readonly IRepositoryClientInterceptor<T>? _specificClientInterceptor;

        public RepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor? clientInterceptor = null,
            IRepositoryClientInterceptor<T>? specificClientInterceptor = null)
        {
            _httpClient = httpClientFactory.CreateClient($"{typeof(T).Name}{Const.HttpClientName}");
            _clientInterceptor = clientInterceptor;
            _specificClientInterceptor = specificClientInterceptor;
        }
        private Task<HttpClient> EnrichedClientAsync(RepositoryMethod api)
        {
            if (_specificClientInterceptor != null)
                return _specificClientInterceptor.EnrichAsync(_httpClient, api);
            else if (_clientInterceptor != null)
                return _clientInterceptor.EnrichAsync(_httpClient, api);
            else
                return Task.FromResult(_httpClient);
        }
        public async Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Delete);
            return (await client.GetFromJsonAsync<TState>($"{nameof(RepositoryMethod.Delete)}?key={key}", cancellationToken))!;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Get);
            return await client.GetFromJsonAsync<T>($"{nameof(RepositoryMethod.Get)}?key={key}", cancellationToken);
        }
        public async Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Exist);
            return (await client.GetFromJsonAsync<TState>($"{nameof(RepositoryMethod.Exist)}?key={key}", cancellationToken))!;
        }
        public async Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Insert);
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethod.Insert)}?key={key}", value, cancellationToken);
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<TState>(cancellationToken: cancellationToken))!;
        }

        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Query);
            var query = $"{nameof(RepositoryMethod.Query)}{options?.ToQuery()}";
            return (await client.GetFromJsonAsync<IEnumerable<T>>(query, cancellationToken))!;
        }

        public async Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Update);
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethod.Update)}?key={key}", value, cancellationToken);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<TState>(cancellationToken: cancellationToken))!;
        }
    }
}