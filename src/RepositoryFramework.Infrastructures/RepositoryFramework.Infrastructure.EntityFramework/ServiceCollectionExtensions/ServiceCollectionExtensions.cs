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
        public static IRepositoryEntityFrameworkBuilder<T, TKey, TEntityModel> AddRepositoryInEntityFramework<T, TKey, TEntityModel, TContext>(
           this IServiceCollection services,
                Action<EntityFrameworkOptions<T, TKey, TEntityModel, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where TEntityModel : class
            where TContext : DbContext
        {
            options.Invoke(EntityFrameworkOptions<T, TKey, TEntityModel, TContext>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey, TEntityModel, TContext>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey, TEntityModel>(services.AddRepository<T, TKey, EntityFrameworkRepository<T, TKey, TEntityModel, TContext>>(ServiceLifetime.Scoped, settings));
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
        public static IRepositoryEntityFrameworkBuilder<T, TKey, TEntityModel> AddCommandInEntityFramework<T, TKey, TEntityModel, TContext>(
           this IServiceCollection services,
                  Action<EntityFrameworkOptions<T, TKey, TEntityModel, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where TEntityModel : class
            where TContext : DbContext
        {
            options.Invoke(EntityFrameworkOptions<T, TKey, TEntityModel, TContext>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey, TEntityModel, TContext>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey, TEntityModel>(services.AddCommand<T, TKey, EntityFrameworkRepository<T, TKey, TEntityModel, TContext>>(ServiceLifetime.Scoped, settings));
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
        public static IRepositoryEntityFrameworkBuilder<T, TKey, TEntityModel> AddQueryInEntityFramework<T, TKey, TEntityModel, TContext>(
           this IServiceCollection services,
                 Action<EntityFrameworkOptions<T, TKey, TEntityModel, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where TEntityModel : class
            where TContext : DbContext
        {
            options.Invoke(EntityFrameworkOptions<T, TKey, TEntityModel, TContext>.Instance);
            services.AddSingleton(EntityFrameworkOptions<T, TKey, TEntityModel, TContext>.Instance);
            return new RepositoryEntityFrameworkBuilder<T, TKey, TEntityModel>(services.AddQuery<T, TKey, EntityFrameworkRepository<T, TKey, TEntityModel, TContext>>(ServiceLifetime.Scoped, settings));
        }
    }
}
