using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Cosmos.Sql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default cosmos sql service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your Cosmos database.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> AddRepositoryInCosmosSql<T, TKey>(
           this IServiceCollection services,
                Action<CosmosSqlConnectionSettings> connectionSettings,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new CosmosSqlConnectionSettings();
            connectionSettings.Invoke(options);
            CosmosSqlServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(new CosmosSettings<T, TKey>(options.ContainerName ?? typeof(T).Name));
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            return new RepositoryCosmosSqlBuilder<T, TKey>(services.AddRepository<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
        /// <summary>
        /// Add a default cosmos sql service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your Cosmos database.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> AddCommandInCosmosSql<T, TKey>(
           this IServiceCollection services,
                Action<CosmosSqlConnectionSettings> connectionSettings,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new CosmosSqlConnectionSettings();
            connectionSettings.Invoke(options);
            CosmosSqlServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(new CosmosSettings<T, TKey>(options.ContainerName ?? typeof(T).Name));
            return new RepositoryCosmosSqlBuilder<T, TKey>(services.AddCommand<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
        /// <summary>
        /// Add a default cosmos sql service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your Cosmos database.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> AddQueryInCosmosSql<T, TKey>(
           this IServiceCollection services,
                Action<CosmosSqlConnectionSettings> connectionSettings,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new CosmosSqlConnectionSettings();
            connectionSettings.Invoke(options);
            CosmosSqlServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(new CosmosSettings<T, TKey>(options.ContainerName ?? typeof(T).Name));
            return new RepositoryCosmosSqlBuilder<T, TKey>(services.AddQuery<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
    }
}
