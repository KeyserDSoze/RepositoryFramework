using RepositoryFramework;
using RepositoryFramework.ApiClient;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static RepositoryBuilder<T, string, bool> AddApiClient<T>(this IServiceCollection services,
            ClientType clientType,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            services.AddHttpClient($"{typeof(T).Name}{Const.HttpClientName}", options =>
            {
                configureClient?.Invoke(options);
                options.BaseAddress = new Uri($"https://{domain}/{startingPath}/{typeof(T).Name}/");
            });
            return clientType switch
            {
                ClientType.Query => services.AddQuery<T, RepositoryClient<T>>(serviceLifetime),
                ClientType.Command => services.AddCommand<T, RepositoryClient<T>>(serviceLifetime),
                _ => services.AddRepository<T, RepositoryClient<T>>(serviceLifetime),
            };
        }
        /// <summary>
        /// Add a Repository Client as IRepository<<typeparamref name="T"/>> with a domain and a starting path
        /// and with a string as key and a bool as state.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static RepositoryBuilder<T, string, bool> AddRepositoryApiClient<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddApiClient<T>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommand<<typeparamref name="T"/>> with a domain and a starting path
        /// and with a string as key and a bool as state.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static RepositoryBuilder<T, string, bool> AddCommandApiClient<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddApiClient<T>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as IQuery<<typeparamref name="T"/>> with a domain and a starting path
        /// and with a string as key and a bool as state.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static RepositoryBuilder<T, string, bool> AddQueryApiClient<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddApiClient<T>(ClientType.Query, domain, startingPath, configureClient, serviceLifetime);
    }
}