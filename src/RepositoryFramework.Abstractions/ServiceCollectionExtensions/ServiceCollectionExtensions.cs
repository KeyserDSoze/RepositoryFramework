using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add repository framework
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepository<T, TKey>(this IServiceCollection services,
          Action<RepositorySettings<T, TKey>> settings)
          where TKey : notnull
        {
            var defaultSettings = new RepositorySettings<T, TKey>(services);
            settings.Invoke(defaultSettings);
            return services;
        }
        /// <summary>
        /// Add repository framework with a storage
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepository<T, TKey, TStorage>(this IServiceCollection services,
              Action<RepositorySettings<T, TKey>>? settings = null,
              ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TKey : notnull
            where TStorage : class, IRepository<T, TKey>
            => services.AddRepository<T, TKey>(x =>
                {
                    settings?.Invoke(x);
                    x.SetRepositoryStorage<TStorage>(serviceLifetime);
                });
        /// <summary>
        /// Add command storage
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommand<T, TKey, TStorage>(this IServiceCollection services,
              Action<RepositorySettings<T, TKey>>? settings = null,
              ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TKey : notnull
            where TStorage : class, ICommand<T, TKey>
            => services.AddRepository<T, TKey>(x =>
            {
                settings?.Invoke(x);
                x.SetCommandStorage<TStorage>(serviceLifetime);
            });
        /// <summary>
        /// Add query storage
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddQuery<T, TKey, TStorage>(this IServiceCollection services,
              Action<RepositorySettings<T, TKey>>? settings = null,
              ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TKey : notnull
            where TStorage : class, IQuery<T, TKey>
            => services.AddRepository<T, TKey>(x =>
            {
                settings?.Invoke(x);
                x.SetQueryStorage<TStorage>(serviceLifetime);
            });
    }
}
