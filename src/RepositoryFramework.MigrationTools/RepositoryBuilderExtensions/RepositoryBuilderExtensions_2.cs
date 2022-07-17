using RepositoryFramework;
using RepositoryFramework.Migration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add migration service, inject the IMigrationManager<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// to set up the data migration methods.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to retrieve, update or delete your data from repository.</typeparam>
        /// <typeparam name="TMigrationSource">Repository pattern for storage that you have to migrate.</typeparam>
        /// <param name="builder">IServiceCollection.</param>
        /// <param name="settings">Settings for migration.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>IServiceCollection</returns>
        public static RepositoryBuilder<T, TKey> AddMigrationSource<T, TKey, TMigrationSource>(this RepositoryBuilder<T, TKey> builder,
            Action<MigrationOptions<T, TKey, State>> settings,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
          where TMigrationSource : class, IMigrationSource<T, TKey>
          where TKey : notnull
        {
            var options = new MigrationOptions<T, TKey, State>();
            settings?.Invoke(options);
            builder.Services
                .AddSingleton(options)
                .AddService<IMigrationSource<T, TKey>, TMigrationSource>(serviceLifetime)
                .AddService<IMigrationManager<T, TKey>, MigrationManager<T, TKey>>(serviceLifetime);
            return builder;
        }
    }
}