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
        /// <typeparam name="TMigrationSource">Repository pattern for storage that you have to migrate.</typeparam>
        /// <param name="builder">IServiceCollection.</param>
        /// <param name="settings">Settings for migration.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<T></returns>
        public static RepositoryBuilder<T> AddMigrationSource<T, TMigrationSource>(this RepositoryBuilder<T> builder,
            Action<MigrationOptions<T, string, State<T>>> settings,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
          where TMigrationSource : class, IMigrationSource<T>
        {
            var options = new MigrationOptions<T, string, State<T>>();
            settings?.Invoke(options);
            builder.Services
                .AddSingleton(options)
                .AddService<IMigrationSource<T>, TMigrationSource>(serviceLifetime)
                .AddService<IMigrationManager<T>, MigrationManager<T>>(serviceLifetime);
            return builder;
        }
    }
}