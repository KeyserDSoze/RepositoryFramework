using Microsoft.Data.SqlClient;
using RepositoryFramework;
using RepositoryFramework.Infrastructure.MsSql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default MsSql service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your MsSql connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryMsSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryMsSqlBuilder<T, TKey> AddRepositoryInMsSql<T, TKey>(
           this IServiceCollection services,
                Action<MsSqlOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            options.Invoke(MsSqlOptions<T, TKey>.Instance);
            services.AddSingleton(MsSqlOptions<T, TKey>.Instance);
            services.AddEventAfterServiceCollectionBuild(serviceProvider => MsSqlCreateTableOrMergeNewColumnsInExistingTableAsync(MsSqlOptions<T, TKey>.Instance));
            return new RepositoryMsSqlBuilder<T, TKey>(services.AddRepository<T, TKey, SqlRepository<T, TKey>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default MsSql service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your MsSql connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryMsSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryMsSqlBuilder<T, TKey> AddCommandInMsSql<T, TKey>(
           this IServiceCollection services,
                  Action<MsSqlOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            options.Invoke(MsSqlOptions<T, TKey>.Instance);
            services.AddSingleton(MsSqlOptions<T, TKey>.Instance);
            services.AddEventAfterServiceCollectionBuild(serviceProvider => MsSqlCreateTableOrMergeNewColumnsInExistingTableAsync(MsSqlOptions<T, TKey>.Instance));
            return new RepositoryMsSqlBuilder<T, TKey>(services.AddCommand<T, TKey, SqlRepository<T, TKey>>(ServiceLifetime.Scoped, settings));
        }
        /// <summary>
        /// Add a default MsSql service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your MsSql connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryMsSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryMsSqlBuilder<T, TKey> AddQueryInMsSql<T, TKey>(
           this IServiceCollection services,
                 Action<MsSqlOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            options.Invoke(MsSqlOptions<T, TKey>.Instance);
            services.AddSingleton(MsSqlOptions<T, TKey>.Instance);
            services.AddEventAfterServiceCollectionBuild(serviceProvider => MsSqlCreateTableOrMergeNewColumnsInExistingTableAsync(MsSqlOptions<T, TKey>.Instance));
            return new RepositoryMsSqlBuilder<T, TKey>(services.AddQuery<T, TKey, SqlRepository<T, TKey>>(ServiceLifetime.Scoped, settings));
        }
    }
}
