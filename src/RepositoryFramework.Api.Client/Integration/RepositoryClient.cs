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
        private static async Task<TResult> PostAsJson<TMessage, TResult>(HttpClient client, string path, TMessage message, CancellationToken cancellationToken)
        {
            var response = await client.PostAsJsonAsync(path, message, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken).NoContext())!;
        }
        private static string GetCorrectUriWithKey(string path, TKey key)
        {
            if (key is IKey keyAsIKey)
                return string.Format(path, keyAsIKey.AsString());
            else
                return string.Format(path, key.ToString());
        }

        public async Task<State<T, TKey>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Delete).NoContext();
            if (s_settings.IsJsonableKey)
                return await PostAsJson<TKey, State<T, TKey>>(client, s_settings.DeletePath, key, cancellationToken).NoContext();
            else
                return (await client.GetFromJsonAsync<State<T, TKey>>(GetCorrectUriWithKey(s_settings.DeletePath, key), cancellationToken).NoContext())!;
        }
        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Get).NoContext();
            if (s_settings.IsJsonableKey)
                return await PostAsJson<TKey, T>(client, s_settings.GetPath, key, cancellationToken).NoContext();
            else
                return await client.GetFromJsonAsync<T>(GetCorrectUriWithKey(s_settings.GetPath, key), cancellationToken).NoContext();
        }
        public async Task<State<T, TKey>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Exist).NoContext();
            if (s_settings.IsJsonableKey)
                return await PostAsJson<TKey, State<T, TKey>>(client, s_settings.ExistPath, key, cancellationToken).NoContext();
            else
                return (await client.GetFromJsonAsync<State<T, TKey>>(GetCorrectUriWithKey(s_settings.ExistPath, key), cancellationToken).NoContext())!;
        }
        public async Task<State<T, TKey>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Insert).NoContext();
            if (s_settings.IsJsonableKey)
                return await PostAsJson<Entity<T, TKey>, State<T, TKey>>(client, s_settings.InsertPath, new(value, key), cancellationToken).NoContext();
            else
                return await PostAsJson<T, State<T, TKey>>(client, GetCorrectUriWithKey(s_settings.InsertPath, key), value, cancellationToken).NoContext();
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
            if (s_settings.IsJsonableKey)
                return await PostAsJson<Entity<T, TKey>, State<T, TKey>>(client, s_settings.UpdatePath, new(value, key), cancellationToken).NoContext();
            else
                return await PostAsJson<T, State<T, TKey>>(client, GetCorrectUriWithKey(s_settings.UpdatePath, key), value, cancellationToken).NoContext();
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
