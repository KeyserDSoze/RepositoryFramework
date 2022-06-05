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
        /// <typeparam name="TMigrationSource">Repository pattern for storage that you have to migrate.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="settings">Settings for migration.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddMigrationSource<T, TMigrationSource>(this IServiceCollection services,
            Action<MigrationOptions<T, string, bool>> settings,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
          where TMigrationSource : class, IMigrationSource<T>
        {
            var options = new MigrationOptions<T, string, bool>();
            settings?.Invoke(options);
            options.CheckIfIsAnOkState = x => x;
            services.AddSingleton(options);
            return services
                .AddService<IMigrationSource<T>, TMigrationSource>(serviceLifetime)
                .AddService<IMigrationManager<T>, MigrationManager<T>>(serviceLifetime);
        }
    }
}