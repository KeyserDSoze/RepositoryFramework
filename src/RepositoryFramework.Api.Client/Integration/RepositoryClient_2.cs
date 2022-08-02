using System.Net.Http.Json;
using System.Text.Json;

namespace RepositoryFramework.Api.Client
{
    internal class RepositoryClient<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
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
        public async Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Delete).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<State<T>>($"{nameof(RepositoryMethods.Delete)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Get).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return await client.GetFromJsonAsync<T>($"{nameof(RepositoryMethods.Get)}?key={keyAsString}", cancellationToken).NoContext();
        }
        public async Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Exist).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<State<T>>($"{nameof(RepositoryMethods.Exist)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Insert).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Insert)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<State<T>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Query).NoContext();
            var query = $"{nameof(RepositoryMethods.Query)}{options?.ToQuery()}";
            return (await client.GetFromJsonAsync<List<T>>(query, cancellationToken).NoContext())!;
        }
        public async ValueTask<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Count).NoContext();
            var query = $"{nameof(RepositoryMethods.Count)}{options?.ToQuery()}";
            return (await client.GetFromJsonAsync<long>(query, cancellationToken).NoContext())!;
        }
        public async Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Update).NoContext();
            string? keyAsString = _hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Update)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<State<T>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Batch).NoContext();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Batch)}", operations, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<BatchResults<T, TKey>>(cancellationToken: cancellationToken).NoContext())!;
        }
    }
}