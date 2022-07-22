using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Web;

namespace RepositoryFramework.Api.Client
{
    internal class RepositoryClient<T, TKey, TState> : IRepositoryPattern<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly HttpClient _httpClient;
        private readonly IRepositoryClientInterceptor? _clientInterceptor;
        private readonly IRepositoryClientInterceptor<T>? _specificClientInterceptor;
        private static readonly bool _hasProperties = typeof(TKey).GetProperties().Length > 0;

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
            var client = await EnrichedClientAsync(RepositoryMethod.Delete).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<TState>($"{nameof(RepositoryMethod.Delete)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Get).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return await client.GetFromJsonAsync<T>($"{nameof(RepositoryMethod.Get)}?key={keyAsString}", cancellationToken).NoContext();
        }
        public async Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Exist).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<TState>($"{nameof(RepositoryMethod.Exist)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Insert).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethod.Insert)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<TState>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Query).NoContext();
            var query = $"{nameof(RepositoryMethod.Query)}{options?.ToQuery()}";
            return (await client.GetFromJsonAsync<IEnumerable<T>>(query, cancellationToken).NoContext())!;
        }
        public async Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Count).NoContext();
            var query = $"{nameof(RepositoryMethod.Count)}{options?.ToQuery()}";
            return (await client.GetFromJsonAsync<long>(query, cancellationToken).NoContext())!;
        }
        public async Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Update).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethod.Update)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<TState>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<List<BatchResult<TKey, TState>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Batch).NoContext();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethod.Batch)}", operations, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<List<BatchResult<TKey, TState>>>(cancellationToken: cancellationToken).NoContext())!;
        }
    }
}