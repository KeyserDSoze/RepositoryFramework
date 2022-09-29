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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2743:Static fields should not be used in generic types", Justification = "Necessary for each T and TKey class.")]
        private static readonly bool s_isJsonable = IsJsonable();
        private static bool IsJsonable()
        {
            var type = typeof(TKey);
            if (type == typeof(string) || type == typeof(Guid) ||
                type == typeof(DateTimeOffset) || type == typeof(TimeSpan) ||
                type == typeof(nint) || type == typeof(nuint))
                return false;
            else
                return type.GetProperties().Length > 0;
        }
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
        public async Task<State<T, TKey>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Delete).NoContext();
            var keyAsString = s_isJsonable ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<State<T, TKey>>($"{nameof(RepositoryMethods.Delete)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Get).NoContext();
            var keyAsString = s_isJsonable ? key.ToJson() : key.ToString();
            return await client.GetFromJsonAsync<T>($"{nameof(RepositoryMethods.Get)}?key={keyAsString}", cancellationToken).NoContext();
        }
        public async Task<State<T, TKey>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Exist).NoContext();
            var keyAsString = s_isJsonable ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<State<T, TKey>>($"{nameof(RepositoryMethods.Exist)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<State<T, TKey>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Insert).NoContext();
            var keyAsString = s_isJsonable ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Insert)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<State<T, TKey>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async IAsyncEnumerable<Entity<T, TKey>> QueryAsync(IFilterExpression filter,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Query).NoContext();
            var value = filter.Serialize();
            var response = await client.PostAsJsonAsync(nameof(RepositoryMethods.Query), value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<Entity<T, TKey>>>(cancellationToken: cancellationToken).NoContext();
            if (result != null)
                foreach (var item in result)
                    if (!cancellationToken.IsCancellationRequested)
                        yield return item;
        }
        public async ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            IFilterExpression filter, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Operation).NoContext();
            var value = filter.Serialize();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Operation)}?op={operation.Name}&returnType={GetPrimitiveNameOrAssemblyQualifiedName()}",
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
            var keyAsString = s_isJsonable ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Update)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<State<T, TKey>>(cancellationToken: cancellationToken).NoContext())!;
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
