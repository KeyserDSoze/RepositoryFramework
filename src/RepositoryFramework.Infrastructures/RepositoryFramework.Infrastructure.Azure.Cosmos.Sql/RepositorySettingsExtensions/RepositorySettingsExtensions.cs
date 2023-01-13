using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Cosmos.Sql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositorySettingsExtensions
    {
        private static IRepositoryCosmosSqlBuilder<T, TKey> WithCosmosSql<T, TKey>(
           this RepositorySettings<T, TKey> settings,
            PatternType type,
            Action<CosmosSqlConnectionSettings> connectionSettings)
            where TKey : notnull
        {
            var options = new CosmosSqlConnectionSettings();
            connectionSettings.Invoke(options);
            CosmosSqlServiceClientFactory.Instance.Add<T>(options);
            settings.Services.AddSingleton(new CosmosSettings<T, TKey>(options.ContainerName ?? typeof(T).Name));
            settings.Services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            settings.SetStorage<CosmosSqlRepository<T, TKey>>(type, ServiceLifetime.Singleton);
            return new RepositoryCosmosSqlBuilder<T, TKey>(settings.Services);
        }
        /// <summary>
        /// Add a default cosmos sql service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your Cosmos database.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> WithCosmosSql<T, TKey>(
           this RepositorySettings<T, TKey> settings,
            Action<CosmosSqlConnectionSettings> connectionSettings)
            where TKey : notnull
            => settings.WithCosmosSql(PatternType.Repository, connectionSettings);
        /// <summary>
        /// Add a default cosmos sql service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your Cosmos database.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> WithCommandInCosmosSql<T, TKey>(
           this RepositorySettings<T, TKey> settings,
                Action<CosmosSqlConnectionSettings> connectionSettings)
            where TKey : notnull
            => settings.WithCosmosSql(PatternType.Command, connectionSettings);
        /// <summary>
        /// Add a default cosmos sql service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your Cosmos database.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> WithQueryInCosmosSql<T, TKey>(
           this RepositorySettings<T, TKey> settings,
                Action<CosmosSqlConnectionSettings> connectionSettings)
            where TKey : notnull
            => settings.WithCosmosSql(PatternType.Query, connectionSettings);
    }
}
