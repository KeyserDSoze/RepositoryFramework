using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add repository storage, inject the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// with a State as TState.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TStorage">Repository pattern storage.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <param name="notExposableAsApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddRepository<T, TKey, TStorage>(this IServiceCollection services,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
          bool notExposableAsApi = false)
          where TStorage : class, IRepositoryPattern<T, TKey>
          where TKey : notnull
        {
            var service = services.SetService<T, TKey>();
            var currentType = typeof(IRepository<T, TKey>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            service.NotExposableAsApi = notExposableAsApi;
            services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(IRepositoryPattern<T, TKey>))
                .AddService<IRepositoryPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<IRepository<T, TKey>, Repository<T, TKey>>(serviceLifetime);
            return new RepositoryBuilder<T, TKey>(services, PatternType.Repository, serviceLifetime);
        }


        /// <summary>
        /// Add Command storage for your CQRS, inject the ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// with a State as TState.
        /// </summary>
        /// <typeparam name="T">Model used for your command.</typeparam>
        /// <typeparam name="TKey">Key to store, update or delete your data.</typeparam>
        /// <typeparam name="TStorage">Command pattern storage.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <param name="notExposableAsApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddCommand<T, TKey, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            bool notExposableAsApi = false)
            where TStorage : class, ICommandPattern<T, TKey>
            where TKey : notnull
        {
            var service = services.SetService<T, TKey>();
            var currentType = typeof(ICommand<T, TKey>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            service.NotExposableAsApi = notExposableAsApi;
            services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(ICommandPattern<T, TKey>))
                .AddService<ICommandPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<ICommand<T, TKey>, Command<T, TKey>>(serviceLifetime);
            return new RepositoryBuilder<T, TKey>(services, PatternType.Command, serviceLifetime);
        }

        /// <summary>
        /// Add Query storage for your CQRS, inject the IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// with a State as TState.
        /// </summary>
        /// <typeparam name="T">Model used for your query.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TStorage">Query pattern storage.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <param name="notExposableAsApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddQuery<T, TKey, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
           bool notExposableAsApi = false)
           where TStorage : class, IQueryPattern<T, TKey>
           where TKey : notnull
        {
            var service = services.SetService<T, TKey>();
            var currentType = typeof(IQuery<T, TKey>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            service.NotExposableAsApi = notExposableAsApi;
            services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(IQueryPattern<T, TKey>))
                .AddService<IQueryPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<IQuery<T, TKey>, Query<T, TKey>>(serviceLifetime);
            return new RepositoryBuilder<T, TKey>(services, PatternType.Query, serviceLifetime);
        }
    }
}
