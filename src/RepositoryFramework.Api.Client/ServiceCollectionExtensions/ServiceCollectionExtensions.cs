using Microsoft.Extensions.DependencyInjection;
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
        private static IServiceCollection AddServiceWithLifeTimeClient<TInterface, TImplementation>(this IServiceCollection services,
          ServiceLifetime serviceLifetime)
            where TImplementation : class, TInterface
        {
            return serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped(typeof(TInterface), typeof(TImplementation)),
                ServiceLifetime.Transient => services.AddTransient(typeof(TInterface), typeof(TImplementation)),
                ServiceLifetime.Singleton => services.AddSingleton(typeof(TInterface), typeof(TImplementation)),
                _ => services.AddScoped(typeof(TInterface), typeof(TImplementation))
            };
        }
        private static IServiceCollection AddRepositoryClient<T, TKey>(this IServiceCollection services,
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
            Type keyType = typeof(TKey);
            if (keyType != typeof(object))
            {
                if (clientType == ClientType.Query)
                    return services.AddServiceWithLifeTimeClient<IQueryClient<T, TKey>, RepositoryClient<T, TKey>>(serviceLifetime);
                else if (clientType == ClientType.Command)
                    return services.AddServiceWithLifeTimeClient<ICommandClient<T, TKey>, RepositoryClient<T, TKey>>(serviceLifetime);
                else
                    return services.AddServiceWithLifeTimeClient<IRepositoryClient<T, TKey>, RepositoryClient<T, TKey>>(serviceLifetime);
            }
            else
            {
                if (clientType == ClientType.Query)
                    return services.AddServiceWithLifeTimeClient<IQueryClient<T>, ObjectRepositoryClient<T>>(serviceLifetime);
                else if (clientType == ClientType.Command)
                    return services.AddServiceWithLifeTimeClient<ICommandClient<T>, ObjectRepositoryClient<T>>(serviceLifetime);
                else
                    return services.AddServiceWithLifeTimeClient<IRepositoryClient<T>, ObjectRepositoryClient<T>>(serviceLifetime);
            }
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
            => services.AddRepositoryClient<T, TKey>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Repository Client as IRepositoryClient<<typeparamref name="T"/>> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryClient<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, object>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Repository Client as IRepositoryClient<<typeparamref name="T"/>, string> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryClientWithStringKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, string>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Repository Client as IRepositoryClient<<typeparamref name="T"/>, Guid> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryClientWithGuidKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, Guid>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Repository Client as IRepositoryClient<<typeparamref name="T"/>, int> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryClientWithIntKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, int>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Repository Client as IRepositoryClient<<typeparamref name="T"/>, long> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryClientWithLongKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, long>(ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
    }
}