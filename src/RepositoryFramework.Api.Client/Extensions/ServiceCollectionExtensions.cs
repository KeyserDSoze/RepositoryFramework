using RepositoryFramework;
using RepositoryFramework.Api.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IApiBuilder<T, TKey> AddApiClient<T, TKey>(this IServiceCollection services,
            PatternType clientType,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TKey : notnull
        {
            return new ApiBuilder<T, TKey>(clientType switch
            {
                PatternType.Query => services.AddQuery<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime, SetOptionsForClient),
                PatternType.Command => services.AddCommand<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime, SetOptionsForClient),
                _ => services.AddRepository<T, TKey, RepositoryClient<T, TKey>>(serviceLifetime, SetOptionsForClient),
            }, services);

            static void SetOptionsForClient(RepositoryFrameworkOptions<T, TKey> options)
            {
                options.HasToTranslate = false;
                options.IsNotExposableAsApi = true;
            }
        }
        /// <summary>
        /// Add a Repository Client as IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IApiBuilder<T, TKey> AddRepositoryApiClient<T, TKey>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(PatternType.Repository, serviceLifetime);
        /// <summary>
        /// Add a Command Client as ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IApiBuilder<T, TKey> AddCommandApiClient<T, TKey>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(PatternType.Command, serviceLifetime);
        /// <summary>
        /// Add a Command Client as IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> with a domain and a starting path
        /// and with a State as TState.
        /// The final url will be https://{domain}/{startingPath}/
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IApiBuilder<T, TKey> AddQueryApiClient<T, TKey>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
            => services.AddApiClient<T, TKey>(PatternType.Query, serviceLifetime);
    }
}
