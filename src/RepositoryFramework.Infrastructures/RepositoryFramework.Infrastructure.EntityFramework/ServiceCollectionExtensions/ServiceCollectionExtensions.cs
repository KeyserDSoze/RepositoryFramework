using RepositoryFramework;
using RepositoryFramework.Infrastructure.EntityFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default Entity Framework service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryEntityFrameworkBuilder<T, TKey> AddRepositoryInMsSql<T, TKey>(
           this IServiceCollection services,
                Action<EntityFrameworkOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
        {
            options.Invoke(EntityFrameworkOptions<T, TKey>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey>(services.AddRepository<T, TKey, EntityFrameworkRepository<T, TKey>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default Entity Framework service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryEntityFrameworkBuilder<T, TKey> AddCommandInMsSql<T, TKey>(
           this IServiceCollection services,
                  Action<EntityFrameworkOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
        {
            options.Invoke(EntityFrameworkOptions<T, TKey>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey>(services.AddCommand<T, TKey, EntityFrameworkRepository<T, TKey>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default Entity Framework service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryEntityFrameworkBuilder<T, TKey> AddQueryInMsSql<T, TKey>(
           this IServiceCollection services,
                 Action<EntityFrameworkOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
        {
            options.Invoke(EntityFrameworkOptions<T, TKey>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey>(services.AddQuery<T, TKey, EntityFrameworkRepository<T, TKey>>(ServiceLifetime.Scoped, settings));
        }
    }
}
