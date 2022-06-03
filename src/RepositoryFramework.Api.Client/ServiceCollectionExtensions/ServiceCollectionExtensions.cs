using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework;
using RepositoryFramework.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private enum ClientType
        {
            Repository,
            Query,
            Command
        }
        private static IServiceCollection AddClient<T, TKey>(this IServiceCollection services,
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
        /// Add a Repository Client as IRepositoryClient<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path.
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
        public static IServiceCollection AddRepositoryClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddClient<T, TKey>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
    }
}