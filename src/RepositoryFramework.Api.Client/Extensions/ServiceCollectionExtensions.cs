using RepositoryFramework;
using RepositoryFramework.Api.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IRepositoryBuilder<T, TKey> AddApiClient<T, TKey>(this IServiceCollection services,
            PatternType clientType,
            string domain,
            string startingPath = "api",
            string? version = default,
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TKey : notnull
        {
            var settingsKey = $"{typeof(T).Name}_{typeof(TKey).Name}";
            RepositoryClientSettings.Instance.Clients.Add(settingsKey,
                new RepositorySingleClientSettings(startingPath, version, typeof(T), typeof(TKey)));
            services.AddHttpClient($"{typeof(T).Name}{Const.HttpClientName}", options =>
            {
                configureClient?.Invoke(options);
                if (options.BaseAddress == null)
                    options.BaseAddress = new Uri($"https://{domain}");
            });
            services.AddSingleton(new RepositoryFrameworkOptions<T, TKey>()
            {
                HasToTranslate = false,
                IsNotExposableAsApi = true
            });
            return clientType switch
            {
                PatternType.Query => services.AddQuery<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime),
                PatternType.Command => services.AddCommand<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime),
                _ => services.AddRepository<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime),
            };
        }
        /// <summary>
        /// Add a Repository Client as IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="version">Api version</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddRepositoryApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            string? version = default,
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(PatternType.Repository, domain, startingPath, version, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="version">Api version</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddCommandApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            string? version = default,
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(PatternType.Command, domain, startingPath, version, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="version">Api version</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddQueryApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            string? version = default,
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(PatternType.Query, domain, startingPath, version, configureClient, serviceLifetime);
    }
}
