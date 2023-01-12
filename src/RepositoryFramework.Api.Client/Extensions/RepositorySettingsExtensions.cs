using RepositoryFramework;
using RepositoryFramework.Api.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositorySettingsExtensions
    {
        private static IRepositoryApiBuilder<T, TKey> AddApiClient<T, TKey>(this IRepositorySettings<T, TKey> settings,
            PatternType clientType,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TKey : notnull
        {
            settings.Services.AddSingleton(ApiClientSettings<T, TKey>.Instance);
            settings.SetNotExposable();
            _ = clientType switch
            {
                PatternType.Query => settings.SetQueryStorage<RepositoryClient<T, TKey>>(serviceLifetime),
                PatternType.Command => settings.SetCommandStorage<RepositoryClient<T, TKey>>(serviceLifetime),
                _ => settings.SetRepositoryStorage<RepositoryClient<T, TKey>>(serviceLifetime),
            };
            return new ApiBuilder<T, TKey>(settings.Services);
        }
        /// <summary>
        /// Add a Repository Client as IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryApiBuilder<T, TKey> WithApiClient<T, TKey>(this IRepositorySettings<T, TKey> settings,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => settings.AddApiClient(PatternType.Repository, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryApiBuilder<T, TKey> WithCommandApiClient<T, TKey>(this IRepositorySettings<T, TKey> settings,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => settings.AddApiClient(PatternType.Command, serviceLifetime);
        /// <summary>
        /// Add a Command Client as IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryApiBuilder<T, TKey> WithQueryApiClient<T, TKey>(this IRepositorySettings<T, TKey> settings,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => settings.AddApiClient(PatternType.Query, serviceLifetime);
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/> client. Interceptor runs before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service.</typeparam>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositorySettings<T, TKey> AddApiClientSpecificInterceptor<T, TKey, TInterceptor>(
            this IRepositorySettings<T, TKey> settings,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T>
            where TKey : notnull
        {
            settings
                .Services
                .AddService<IRepositoryClientInterceptor<T>, TInterceptor>(serviceLifetime);
            return settings;
        }
    }
}
