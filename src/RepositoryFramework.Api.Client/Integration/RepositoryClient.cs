using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RepositoryFramework.Api.Client
{
    internal sealed class RepositoryClient<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly HttpClient _httpClient;
        private readonly IRepositoryClientInterceptor? _clientInterceptor;
        private readonly IRepositoryClientInterceptor<T>? _specificClientInterceptor;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2743:Static fields should not be used in generic types", Justification = "Needed for implementation of different keys and models.")]
        private static readonly RepositorySingleClientSettings s_settings = GetSettings();
        private static RepositorySingleClientSettings GetSettings()
        {
            var settingsKey = $"{typeof(T).Name}_{typeof(TKey).Name}";
            return RepositoryClientSettings.Instance.Clients[settingsKey];
        }
        public RepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor? clientInterceptor = null,
            IRepositoryClientInterceptor<T>? specificClientInterceptor = null)
        {
            var name = typeof(T).Name;
            _httpClient = httpClientFactory.CreateClient($"{name}{Const.HttpClientName}");
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
        public async Task<State<T, TKey>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Delete).NoContext();
            var keyAsString = s_settings.IsJsonableKey ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<State<T, TKey>>(string.Format(s_settings.DeletePath, keyAsString), cancellationToken).NoContext())!;
        }
        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Get).NoContext();
            var keyAsString = s_settings.IsJsonableKey ? key.ToJson() : key.ToString();
            return await client.GetFromJsonAsync<T>(string.Format(s_settings.GetPath, keyAsString), cancellationToken).NoContext();
        }
        public async Task<State<T, TKey>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Exist).NoContext();
            var keyAsString = s_settings.IsJsonableKey ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<State<T, TKey>>(string.Format(s_settings.ExistPath, keyAsString), cancellationToken).NoContext())!;
        }
        public async Task<State<T, TKey>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Insert).NoContext();
            var keyAsString = s_settings.IsJsonableKey ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync(string.Format(s_settings.InsertPath, keyAsString), value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<State<T, TKey>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async IAsyncEnumerable<Entity<T, TKey>> QueryAsync(IFilterExpression filter,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Query).NoContext();
            var value = filter.Serialize();
            var response = await client.PostAsJsonAsync(s_settings.QueryPath, value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<Entity<T, TKey>>>(cancellationToken: cancellationToken).NoContext();
            if (result != null)
                foreach (var item in result)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return item;
                }
        }
        public async ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            IFilterExpression filter, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Operation).NoContext();
            var value = filter.Serialize();
            var response = await client.PostAsJsonAsync(
                string.Format(s_settings.OperationPath, operation.Name, GetPrimitiveNameOrAssemblyQualifiedName()),
                value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TProperty>(cancellationToken: cancellationToken).NoContext();
            return result!;

            string? GetPrimitiveNameOrAssemblyQualifiedName()
            {
                var name = operation.Type.AssemblyQualifiedName;
                if (name == null)
                    return null;
                if (PrimitiveMapper.Instance.FromAssemblyQualifiedNameToName.ContainsKey(name))
                    return PrimitiveMapper.Instance.FromAssemblyQualifiedNameToName[name];
                return name;
            }
        }
        public async Task<State<T, TKey>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Update).NoContext();
            var keyAsString = s_settings.IsJsonableKey ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync(string.Format(s_settings.UpdatePath, keyAsString), value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<State<T, TKey>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Batch).NoContext();
            var response = await client.PostAsJsonAsync(s_settings.BatchPath, operations, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<BatchResults<T, TKey>>(cancellationToken: cancellationToken).NoContext())!;
        }
    }
}
