﻿using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace RepositoryFramework.Client
{
    internal class RepositoryClient<T, TKey> : IRepositoryPattern<T, TKey>, IQueryPattern<T, TKey>, ICommandPattern<T, TKey>
        where TKey : notnull
    {
        private readonly HttpClient _httpClient;
        private readonly IRepositoryClientInterceptor? _clientInterceptor;
        private readonly IRepositoryClientInterceptor<T, TKey>? _specificClientInterceptor;

        public RepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor clientInterceptor = null!,
            IRepositoryClientInterceptor<T, TKey> specificClientInterceptor = null!)
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
        public async Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Delete);
            return await client.GetFromJsonAsync<bool>($"{nameof(RepositoryMethod.Delete)}?key={key}", cancellationToken);
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Get);
            return await client.GetFromJsonAsync<T>($"{nameof(RepositoryMethod.Get)}?key={key}", cancellationToken);
        }
        public async Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Exist);
            return await client.GetFromJsonAsync<bool>($"{nameof(RepositoryMethod.Exist)}?key={key}", cancellationToken);
        }
        public async Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Insert);
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethod.Insert)}?key={key}", value, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response!.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }
        private const string LogicAnd = "&";
        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Query);
            var query = new StringBuilder(nameof(RepositoryMethod.Query));
            if (predicate != null || top != null || skip != null)
                query.Append('?');
            if (predicate != null)
                query.Append($"query={HttpUtility.UrlEncode(predicate.ToString())}");
            if (top != null)
                query.Append($"{(predicate == null ? string.Empty : LogicAnd)}top={top}");
            if (skip != null)
                query.Append($"{(predicate == null && top == null ? string.Empty : LogicAnd)}skip={skip}");
            return (await client.GetFromJsonAsync<IEnumerable<T>>(query.ToString(), cancellationToken))!;
        }

        public async Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var client = await EnrichedClientAsync(RepositoryMethod.Update);
            var response = await client.PostAsJsonAsync($"{nameof(RepositoryMethod.Update)}?key={key}", value, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }
    }
}