namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueryClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddRepositoryPatternClient<T, TKey>(false, ClientType.Query, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddQueryClientWithStringKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, string>(true, ClientType.Query, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddQueryClientWithGuidKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, Guid>(true, ClientType.Query, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddQueryClientWithIntKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, int>(true, ClientType.Query, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddQueryClientWithLongKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryPatternClient<T, long>(true, ClientType.Query, domain, startingPath, configureClient, serviceLifetime);
    }
}