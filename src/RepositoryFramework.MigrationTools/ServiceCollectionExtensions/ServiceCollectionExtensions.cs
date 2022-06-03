using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework;
using RepositoryFramework.Migration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add migration service, inject the IMigrationManager<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// to set up the data migration methods.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to retrieve, update or delete your data from repository.</typeparam>
        /// <typeparam name="TStorageToMigrate">Repository pattern for storage that you have to migrate.</typeparam>
        /// <typeparam name="TFinalStorage">Repository pattern for storage where the data migrates on.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="settings">Settings for migration.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddMigrationSource<T, TKey, TStorageToMigrate>(this IServiceCollection services,
            Action<MigrationOptions<T, TKey>> settings,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
          where TStorageToMigrate : class, IMigrationSource<T, TKey>
          where TKey : notnull
        {
            var options = new MigrationOptions<T, TKey>();
            settings?.Invoke(options);
            services.AddSingleton(options);
            return services
                .AddService<IMigrationSource<T, TKey>, TStorageToMigrate>(serviceLifetime)
                .AddService<IMigrationManager<T, TKey>, MigrationManager<T, TKey>>(serviceLifetime);
        }
    }
}