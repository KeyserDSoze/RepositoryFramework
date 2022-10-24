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
        /// <typeparam name="TEntityModel">Model user for your entity framework integration</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TEntityModel"/>></returns>
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
            Check<T, TKey, TEntityModel, TContext>();
            return new RepositoryEntityFrameworkBuilder<T, TKey, TEntityModel>(services.AddRepository<T, TKey, EntityFrameworkRepository<T, TKey, TEntityModel, TContext>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default Entity Framework service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TEntityModel">Model user for your entity framework integration</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TEntityModel"/>></returns>
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
            Check<T, TKey, TEntityModel, TContext>();
            return new RepositoryEntityFrameworkBuilder<T, TKey, TEntityModel>(services.AddCommand<T, TKey, EntityFrameworkRepository<T, TKey, TEntityModel, TContext>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default Entity Framework service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TEntityModel">Model user for your entity framework integration</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryEntityFrameworkBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TEntityModel"/>></returns>
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
            Check<T, TKey, TEntityModel, TContext>();
            return new RepositoryEntityFrameworkBuilder<T, TKey, TEntityModel>(services.AddQuery<T, TKey, EntityFrameworkRepository<T, TKey, TEntityModel, TContext>>(ServiceLifetime.Scoped, settings));
        }
        private static void Check<T, TKey, TEntityModel, TContext>()
            where TKey : notnull
            where TEntityModel : class
            where TContext : DbContext
        {
            if (EntityFrameworkOptions<T, TKey, TEntityModel, TContext>.Instance.DbSet == null)
                throw new ArgumentException($"DbSet not configured in option during {nameof(AddRepositoryInEntityFramework)} method for {typeof(TEntityModel).Name} for model {typeof(T).Name} and key {typeof(TKey).Name}");
        }
    }
}
