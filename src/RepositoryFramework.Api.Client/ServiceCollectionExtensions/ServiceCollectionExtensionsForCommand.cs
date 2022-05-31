namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a Command Client as ICommandClient<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path.
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
        public static IServiceCollection AddCommandClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddRepositoryClient<T, TKey>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommandClient<<typeparamref name="T"/>> with a domain and a starting path.
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
        public static IServiceCollection AddCommandClient<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, object>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommandClient<<typeparamref name="T"/>, string> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommandClientWithStringKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, string>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommandClient<<typeparamref name="T"/>, Guid> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommandClientWithGuidKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, Guid>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommandClient<<typeparamref name="T"/>, int> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommandClientWithIntKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, int>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommandClient<<typeparamref name="T"/>, long> with a domain and a starting path.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="domain">The api domain</param>
        /// <param name="startingPath">Path after domain to compose the final api url</param>
        /// <param name="configureClient">Add custom configuration for HttpClient used by IRepositoryClient</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommandClientWithLongKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, long>(ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
    }
}