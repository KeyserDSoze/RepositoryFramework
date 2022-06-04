using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add repository storage, inject the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// with a bool as state.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to retrieve, update or delete your data from repository</typeparam>
        /// <typeparam name="TStorage">Repository pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepository<T, TKey, TStorage>(this IServiceCollection services,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
          where TStorage : class, IRepositoryPattern<T, TKey>
          where TKey : notnull
        {
            var service = services.SetService<T, TKey>();
            service.RepositoryType = typeof(IRepositoryPattern<T, TKey>);
            return services
                .AddService<IRepositoryPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<IRepository<T, TKey>, Repository<T, TKey>>(serviceLifetime);
        }

        /// <summary>
        /// Add Command storage for your CQRS, inject the ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// with a bool as state.
        /// </summary>
        /// <typeparam name="T">Model used for your command</typeparam>
        /// <typeparam name="TKey">Key to store, update or delete your data</typeparam>
        /// <typeparam name="TStorage">Command pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommand<T, TKey, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, ICommandPattern<T, TKey>
            where TKey : notnull
        {
            var service = services.SetService<T, TKey>();
            service.CommandType = typeof(ICommandPattern<T, TKey>);
            return services
                .AddService<ICommandPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<ICommand<T, TKey>, Command<T, TKey>>(serviceLifetime);
        }

        /// <summary>
        /// Add Query storage for your CQRS, inject the IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// with a bool as state.
        /// </summary>
        /// <typeparam name="T">Model used for your query</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TStorage">Query pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddQuery<T, TKey, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IQueryPattern<T, TKey>
           where TKey : notnull
        {
            var service = services.SetService<T, TKey>();
            service.QueryType = typeof(IQueryPattern<T, TKey>);
            return services
                .AddService<IQueryPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<IQuery<T, TKey>, Query<T, TKey>>(serviceLifetime);
        }
    }
}