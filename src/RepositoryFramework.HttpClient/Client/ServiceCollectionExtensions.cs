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
        internal const string HttpClientName = "RepositoryClient";
        private static IServiceCollection AddRepositoryClient<T, TKey>(this IServiceCollection services,
            bool specificClient,
            ClientType clientType,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TKey : notnull
        {
            services.AddHttpClient($"{typeof(T).Name}{HttpClientName}", options =>
            {
                configureClient?.Invoke(options);
                options.BaseAddress = new Uri($"https://{domain}/{startingPath}/{typeof(T).Name.ToLower()}/");
            });
            Type keyType = typeof(TKey);
            if (clientType == ClientType.Query)
            {
                if (specificClient)
                {
                    if (keyType == typeof(Guid))
                        return services.AddServiceWithLifeTimeClient<IGuidableQueryClient<T>, GuidRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(string))
                        return services.AddServiceWithLifeTimeClient<IStringableQueryClient<T>, StringRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(int))
                        return services.AddServiceWithLifeTimeClient<IIntableQueryClient<T>, IntRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(long))
                        return services.AddServiceWithLifeTimeClient<ILongableQueryClient<T>, LongRepositoryClient<T>>(serviceLifetime);
                }
                return services.AddServiceWithLifeTimeClient<IQueryClient<T, TKey>, RepositoryClient<T, TKey>>(serviceLifetime);
            }
            else if (clientType == ClientType.Command)
            {
                if (specificClient)
                {
                    if (keyType == typeof(Guid))
                        return services.AddServiceWithLifeTimeClient<IGuidableCommandClient<T>, GuidRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(string))
                        return services.AddServiceWithLifeTimeClient<IStringableCommandClient<T>, StringRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(int))
                        return services.AddServiceWithLifeTimeClient<IIntableCommandClient<T>, IntRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(long))
                        return services.AddServiceWithLifeTimeClient<ILongableCommandClient<T>, LongRepositoryClient<T>>(serviceLifetime);
                }
                return services.AddServiceWithLifeTimeClient<ICommandClient<T, TKey>, RepositoryClient<T, TKey>>(serviceLifetime);
            }
            else
            {
                if (specificClient)
                {
                    if (keyType == typeof(Guid))
                        return services.AddServiceWithLifeTimeClient<IGuidableRepositoryClient<T>, GuidRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(string))
                        return services.AddServiceWithLifeTimeClient<IStringableRepositoryClient<T>, StringRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(int))
                        return services.AddServiceWithLifeTimeClient<IIntableRepositoryClient<T>, IntRepositoryClient<T>>(serviceLifetime);
                    else if (keyType == typeof(long))
                        return services.AddServiceWithLifeTimeClient<ILongableRepositoryClient<T>, LongRepositoryClient<T>>(serviceLifetime);
                }
                return services.AddServiceWithLifeTimeClient<IRepositoryClient<T, TKey>, RepositoryClient<T, TKey>>(serviceLifetime);
            }
        }
        public static IServiceCollection AddRepositoryClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull
            => services.AddRepositoryClient<T, TKey>(false, ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddRepositoryClientWithStringKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, string>(true, ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddRepositoryClientWithGuidKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, Guid>(true, ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddRepositoryClientWithIntKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, int>(true, ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
        public static IServiceCollection AddRepositoryClientWithLongKey<T>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddRepositoryClient<T, long>(true, ClientType.Repository, domain, startingPath, configureClient, serviceLifetime);
    }
}