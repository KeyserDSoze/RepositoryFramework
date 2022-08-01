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
        private Task<HttpClient> EnrichedClientAsync(RepositoryMethods api)
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
            var client = await EnrichedClientAsync(RepositoryMethods.Delete).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<TState>($"{nameof(RepositoryMethods.Delete)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Get).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return await client.GetFromJsonAsync<T>($"{nameof(RepositoryMethods.Get)}?key={keyAsString}", cancellationToken).NoContext();
        }
        public async Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Exist).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<TState>($"{nameof(RepositoryMethods.Exist)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Insert).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Insert)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<TState>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Query).NoContext();
            var query = $"{nameof(RepositoryMethods.Query)}{options?.ToQuery()}";
            return (await client.GetFromJsonAsync<IEnumerable<T>>(query, cancellationToken).NoContext())!;
        }
        public async Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Count).NoContext();
            var query = $"{nameof(RepositoryMethods.Count)}{options?.ToQuery()}";
            return (await client.GetFromJsonAsync<long>(query, cancellationToken).NoContext())!;
        }
        public async Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Update).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Update)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<TState>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<BatchResults<TKey, TState>> BatchAsync(BatchOperations<T, TKey, TState> operations, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Batch).NoContext();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Batch)}", operations, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<BatchResults<TKey, TState>>(cancellationToken: cancellationToken).NoContext())!;
        }
    }
}