using System;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Api.Client;

namespace RepositoryFramework
{
    internal sealed class ApiBuilder<T, TKey> : IApiBuilder<T, TKey>
        where TKey : notnull
    {
        public IServiceCollection Services { get; }
        public IRepositoryBuilder<T, TKey> Builder { get; }
        public ApiBuilder(IRepositoryBuilder<T, TKey> builder, IServiceCollection services)
        {
            Builder = builder;
            Services = services;
        }
        public IApiBuilder<T, TKey> WithVersion(string version)
        {
            ApiClientSettings<T, TKey>.Instance.RefreshPath(version: version);
            return this;
        }
        public IApiBuilder<T, TKey> WithName(string name)
        {
            ApiClientSettings<T, TKey>.Instance.RefreshPath(name: name);
            return this;
        }
        public IApiBuilder<T, TKey> WithStartingPath(string path)
        {
            ApiClientSettings<T, TKey>.Instance.RefreshPath(startingPath: path);
            return this;
        }
        public IHttpClientBuilder<T, TKey> WithHttpClient(string domain)
        {
            var httpClientService = Services.AddHttpClient($"{typeof(T).Name}{Const.HttpClientName}", options =>
            {
                options.BaseAddress = new Uri($"https://{domain}");
            });
            return new HttpClientBuilder<T, TKey>(this, httpClientService);
        }
        public IHttpClientBuilder<T, TKey> WithHttpClient(Action<HttpClient> configuredClient)
        {
            var httpClientService = Services.AddHttpClient($"{typeof(T).Name}{Const.HttpClientName}", options =>
            {
                configuredClient?.Invoke(options);
            });
            return new HttpClientBuilder<T, TKey>(this, httpClientService);
        }
    }
}
