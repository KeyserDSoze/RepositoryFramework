namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddRepositoryClient<T, TKey>(false, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithStringKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, string>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithGuidKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, Guid>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithIntKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, int>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithLongKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, long>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
    }
}