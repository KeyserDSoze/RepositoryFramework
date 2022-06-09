using RepositoryFramework;
using RepositoryFramework.Api.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static RepositoryBuilder<T, TKey, bool> AddApiClient<T, TKey>(this IServiceCollection services,
            ClientType clientType,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TKey : notnull
        {
            services.AddHttpClient($"{typeof(T).Name}{Const.HttpClientName}", options =>
            {
                configureClient?.Invoke(options);
                options.BaseAddress = new Uri($"https://{domain}/{startingPath}/{typeof(T).Name}/");
            });
            return clientType switch
            {
                ClientType.Query => services.AddQuery<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime),
                ClientType.Command => services.AddCommand<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime),
                _ => services.AddRepository<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime),
            };
        }
        /// <summary>
        /// Add a Repository Client as IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a bool as state.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static RepositoryBuilder<T, TKey, bool> AddRepositoryApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a bool as state.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static RepositoryBuilder<T, TKey, bool> AddCommandApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a bool as state.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static RepositoryBuilder<T, TKey, bool> AddQueryApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(ClientType.Query, domain, startingPath, configureClient, serviceLifetime);
    }
}