using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add repository storage, inject the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>>
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to retrieve, update or delete your data from repository</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <typeparam name="TStorage">Repository pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepository<T, TKey, TState, TStorage>(this IServiceCollection services,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
          where TStorage : class, IRepositoryPattern<T, TKey, TState>
          where TKey : notnull
        {
            var service = services.SetService<T, TKey, TState>();
            service.RepositoryType = typeof(IRepositoryPattern<T, TKey, TState>);
            return services
                .AddService<IRepositoryPattern<T, TKey, TState>, TStorage>(serviceLifetime)
                .AddService<IRepository<T, TKey, TState>, Repository<T, TKey, TState>>(serviceLifetime);
        }

        /// <summary>
        /// Add Command storage for your CQRS, inject the ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>>
        /// </summary>
        /// <typeparam name="T">Model used for your command</typeparam>
        /// <typeparam name="TKey">Key to store, update or delete your data</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <typeparam name="TStorage">Command pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommand<T, TKey, TState, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, ICommandPattern<T, TKey, TState>
            where TKey : notnull
        {
            var service = services.SetService<T, TKey, TState>();
            service.CommandType = typeof(ICommandPattern<T, TKey, TState>);
            return services
                .AddService<ICommandPattern<T, TKey, TState>, TStorage>(serviceLifetime)
                .AddService<ICommand<T, TKey, TState>, Command<T, TKey, TState>>(serviceLifetime);
        }

        /// <summary>
        /// Add Query storage for your CQRS, inject the IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>>
        /// </summary>
        /// <typeparam name="T">Model used for your query</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <typeparam name="TStorage">Query pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddQuery<T, TKey, TState, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IQueryPattern<T, TKey, TState>
           where TKey : notnull
        {
            var service = services.SetService<T, TKey, TState>();
            service.QueryType = typeof(IQueryPattern<T, TKey, TState>);
            return services
                .AddService<IQueryPattern<T, TKey, TState>, TStorage>(serviceLifetime)
                .AddService<IQuery<T, TKey, TState>, Query<T, TKey, TState>>(serviceLifetime);
        }
    }
}