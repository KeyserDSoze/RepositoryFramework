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
            => services.AddRepositoryPatternClient<T, TKey>(false, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithStringKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, string>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithGuidKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, Guid>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithIntKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, int>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddCommandClientWithLongKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, long>(true, ClientType.Command, domain, startingPath, configureClient, serviceLifetime);
    }
}