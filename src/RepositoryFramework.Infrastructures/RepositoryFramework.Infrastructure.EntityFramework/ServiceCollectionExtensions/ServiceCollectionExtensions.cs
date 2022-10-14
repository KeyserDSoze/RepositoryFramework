using Microsoft.EntityFrameworkCore;
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
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryEntityFrameworkBuilder<T, TKey> AddRepositoryInEntityFramework<T, TKey, TContext>(
           this IServiceCollection services,
                Action<EntityFrameworkOptions<T, TKey, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            options.Invoke(EntityFrameworkOptions<T, TKey, TContext>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey, TContext>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey>(services.AddRepository<T, TKey, EntityFrameworkRepository<T, TKey, TContext>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default Entity Framework service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryEntityFrameworkBuilder<T, TKey> AddCommandInEntityFramework<T, TKey, TContext>(
           this IServiceCollection services,
                  Action<EntityFrameworkOptions<T, TKey, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            options.Invoke(EntityFrameworkOptions<T, TKey, TContext>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey, TContext>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey>(services.AddCommand<T, TKey, EntityFrameworkRepository<T, TKey, TContext>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default Entity Framework service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryEntityFrameworkBuilder<T, TKey> AddQueryInEntityFramework<T, TKey, TContext>(
           this IServiceCollection services,
                 Action<EntityFrameworkOptions<T, TKey, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            options.Invoke(EntityFrameworkOptions<T, TKey, TContext>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey, TContext>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey>(services.AddQuery<T, TKey, EntityFrameworkRepository<T, TKey, TContext>>(ServiceLifetime.Scoped, settings));
        }
    }
}
