using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add repository storage, inject the IRepository<<typeparamref name="T"/>>
        /// with a string as key and a State as TState.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TStorage">Repository pattern storage.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <param name="notExposableAsApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> AddRepository<T, TStorage>(this IServiceCollection services,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
          bool notExposableAsApi = false)
          where TStorage : class, IRepositoryPattern<T>
        {
            var service = services.SetService<T>();
            var currentType = typeof(IRepository<T>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            service.NotExposableAsApi = notExposableAsApi;
            services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(IRepositoryPattern<T>))
                .AddService<IRepositoryPattern<T>, TStorage>(serviceLifetime)
                .AddService<IRepository<T>, Repository<T>>(serviceLifetime);
            return new RepositoryBuilder<T>(services, PatternType.Repository, serviceLifetime);
        }

        /// <summary>
        /// Add Command storage for your CQRS, inject the ICommand<<typeparamref name="T"/>>
        /// with a string as key and a State as TState.
        /// </summary>
        /// <typeparam name="T">Model used for your command.</typeparam>
        /// <typeparam name="TStorage">Command pattern storage.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <param name="notExposableAsApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> AddCommand<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            bool notExposableAsApi = false)
            where TStorage : class, ICommandPattern<T>
        {
            var service = services.SetService<T>();
            var currentType = typeof(ICommand<T>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            service.NotExposableAsApi = notExposableAsApi;
            services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(ICommandPattern<T>))
                .AddService<ICommandPattern<T>, TStorage>(serviceLifetime)
                .AddService<ICommand<T>, Command<T>>(serviceLifetime);
            return new RepositoryBuilder<T>(services, PatternType.Command, serviceLifetime);
        }

        /// <summary>
        /// Add Query storage for your CQRS, inject the IQuery<<typeparamref name="T"/>>
        /// with a string as key and a State as TState.
        /// </summary>
        /// <typeparam name="T">Model used for your query.</typeparam>
        /// <typeparam name="TStorage">Query pattern storage.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <param name="notExposableAsApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> AddQuery<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
           bool notExposableAsApi = false)
           where TStorage : class, IQueryPattern<T>
        {
            var service = services.SetService<T>();
            var currentType = typeof(IQuery<T>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            service.NotExposableAsApi = notExposableAsApi;
            services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(IQueryPattern<T>))
                .AddService<IQueryPattern<T>, TStorage>(serviceLifetime)
                .AddService<IQuery<T>, Query<T>>(serviceLifetime);
            return new RepositoryBuilder<T>(services, PatternType.Query, serviceLifetime);
        }
    }
}