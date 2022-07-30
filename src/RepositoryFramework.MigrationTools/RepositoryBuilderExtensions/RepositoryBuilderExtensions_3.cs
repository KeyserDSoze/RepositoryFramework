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
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <typeparam name="TMigrationSource">Repository pattern for storage that you have to migrate.</typeparam>
        /// <typeparam name="TFinalStorage">Repository pattern for storage where the data migrates on.</typeparam>
        /// <param name="builder">IServiceCollection.</param>
        /// <param name="settings">Settings for migration.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>IServiceCollection</returns>
        public static IRepositoryBuilder<T, TKey, TState> AddMigrationSource<T, TKey, TState, TMigrationSource>(this IRepositoryBuilder<T, TKey, TState> builder,
            Action<MigrationOptions<T, TKey, TState>> settings,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TMigrationSource : class, IMigrationSource<T, TKey, TState>
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            var options = new MigrationOptions<T, TKey, TState>();
            settings?.Invoke(options);
            builder.Services
                .AddSingleton(options)
                .AddService<IMigrationSource<T, TKey, TState>, TMigrationSource>(serviceLifetime)
                .AddService<IMigrationManager<T, TKey, TState>, MigrationManager<T, TKey, TState>>(serviceLifetime);
            return builder;
        }
    }
}