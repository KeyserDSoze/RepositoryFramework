using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add repository storage, inject the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
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
            return serviceLifetime switch
            {
                ServiceLifetime.Transient => services
                    .AddTransient<IRepositoryPattern<T, TKey>, TStorage>()
                    .AddTransient<IRepository<T, TKey>, Repository<T, TKey>>(),
                ServiceLifetime.Singleton => services
                    .AddSingleton<IRepositoryPattern<T, TKey>, TStorage>()
                    .AddSingleton<IRepository<T, TKey>, Repository<T, TKey>>(),
                _ => services
                    .AddScoped<IRepositoryPattern<T, TKey>, TStorage>()
                    .AddScoped<IRepository<T, TKey>, Repository<T, TKey>>(),
            };
        }

        /// <summary>
        /// Add Command storage for your CQRS, inject the ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
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
            return serviceLifetime switch
            {
                ServiceLifetime.Transient => services
                    .AddTransient<ICommandPattern<T, TKey>, TStorage>()
                    .AddTransient<ICommand<T, TKey>, Command<T, TKey>>(),
                ServiceLifetime.Singleton => services
                    .AddSingleton<ICommandPattern<T, TKey>, TStorage>()
                    .AddSingleton<ICommand<T, TKey>, Command<T, TKey>>(),
                _ => services
                    .AddScoped<ICommandPattern<T, TKey>, TStorage>()
                    .AddScoped<ICommand<T, TKey>, Command<T, TKey>>(),
            };
        }

        /// <summary>
        /// Add Query storage for your CQRS, inject the IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
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
            return serviceLifetime switch
            {
                ServiceLifetime.Transient => services
                    .AddTransient<IQueryPattern<T, TKey>, TStorage>()
                    .AddTransient<IQuery<T, TKey>, Query<T, TKey>>(),
                ServiceLifetime.Singleton => services
                    .AddSingleton<IQueryPattern<T, TKey>, TStorage>()
                    .AddSingleton<IQuery<T, TKey>, Query<T, TKey>>(),
                _ => services
                    .AddScoped<IQueryPattern<T, TKey>, TStorage>()
                    .AddScoped<IQuery<T, TKey>, Query<T, TKey>>(),
            };
        }
        private static RepositoryFrameworkService SetService<T, TKey>(this IServiceCollection services)
            where TKey : notnull
        {
            Type entityType = typeof(T);
            Type keyType = typeof(TKey);
            var service = RepositoryFrameworkRegistry.Instance.Services.FirstOrDefault(x => x.ModelType == entityType);
            if (service == null)
            {
                service = new RepositoryFrameworkService { ModelType = entityType };
                RepositoryFrameworkRegistry.Instance.Services.Add(service);
                services.AddSingleton(RepositoryFrameworkRegistry.Instance);
            }
            service.KeyType = keyType;
            return service;
        }
    }
}