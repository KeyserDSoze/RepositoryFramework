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
        private static readonly bool s_hasProperties = typeof(TKey).GetProperties().Length > 0;

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
        public async Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Delete).NoContext();
            var keyAsString = s_hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<HttpClientState<T>>($"{nameof(RepositoryMethods.Delete)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Get).NoContext();
            var keyAsString = s_hasProperties ? key.ToJson() : key.ToString();
            return await client.GetFromJsonAsync<T>($"{nameof(RepositoryMethods.Get)}?key={keyAsString}", cancellationToken).NoContext();
        }
        public async Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Exist).NoContext();
            var keyAsString = s_hasProperties ? key.ToJson() : key.ToString();
            return (await client.GetFromJsonAsync<HttpClientState<T>>($"{nameof(RepositoryMethods.Exist)}?key={keyAsString}", cancellationToken).NoContext())!;
        }
        public async Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Insert).NoContext();
            var keyAsString = s_hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Insert)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response!.Content.ReadFromJsonAsync<HttpClientState<T>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(IFilterExpression filter,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Query).NoContext();
            var value = filter.Serialize();
            var response = await client.PostAsJsonAsync(nameof(RepositoryMethods.Query), value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<HttpClientEntity<T, TKey>>>(cancellationToken: cancellationToken).NoContext();
            if (result != null)
                foreach (var item in result)
                    if (!cancellationToken.IsCancellationRequested)
                        yield return item;
        }
        private async ValueTask<TProperty> OperationAsync<TProperty>(
            RepositoryMethods repositoryMethods,
            IFilterExpression filter,
            CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(repositoryMethods).NoContext();
            var value = filter.Serialize();
            var response = await client.PostAsJsonAsync($"{repositoryMethods}?returnType={GetPrimitiveNameOrAssemblyQualifiedName()}",
                value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TProperty>(cancellationToken: cancellationToken).NoContext();
            return result!;

            string? GetPrimitiveNameOrAssemblyQualifiedName()
            {
                var name = typeof(TProperty).AssemblyQualifiedName;
                if (name == null)
                    return null;
                if (PrimitiveMapper.Instance.FromAssemblyQualifiedNameToName.ContainsKey(name))
                    return PrimitiveMapper.Instance.FromAssemblyQualifiedNameToName[name];
                return name;
            }
        }
        public async Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Update).NoContext();
            var keyAsString = s_hasProperties ? key.ToJson() : key.ToString();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Update)}?key={keyAsString}", value, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<HttpClientState<T>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethods.Batch).NoContext();
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethods.Batch)}", operations, cancellationToken).NoContext();
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<BatchResults<T, TKey>>(cancellationToken: cancellationToken).NoContext())!;
        }
        public ValueTask<TProperty> CountAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => OperationAsync<TProperty>(RepositoryMethods.Count, filter, cancellationToken);
        public ValueTask<TProperty> SumAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => OperationAsync<TProperty>(RepositoryMethods.Sum, filter, cancellationToken);
        public ValueTask<TProperty> MaxAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => OperationAsync<TProperty>(RepositoryMethods.Max, filter, cancellationToken);
        public ValueTask<TProperty> MinAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => OperationAsync<TProperty>(RepositoryMethods.Min, filter, cancellationToken);
        public ValueTask<TProperty> AverageAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => OperationAsync<TProperty>(RepositoryMethods.Average, filter, cancellationToken);
        public async IAsyncEnumerable<IGrouping<TProperty, IEntity<T, TKey>>> GroupByAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
        {
            var results = await OperationAsync<List<IGrouping<TProperty, IEntity<T, TKey>>>>(RepositoryMethods.GroupBy, filter, cancellationToken);
            foreach (var item in results)
            {
                yield return item;
            }
        }
    }
}
