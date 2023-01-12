using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Storage.Table;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositorySettingsExtensions
    {
        private static IRepositoryTableStorageBuilder<T, TKey> AddRepositoryInTableStorage<T, TKey>(
          this IRepositorySettings<T, TKey> settings,
          PatternType type,
          Action<TableStorageConnectionSettings> connectionSettings)
           where TKey : notnull
        {
            var options = new TableStorageConnectionSettings();
            connectionSettings.Invoke(options);
            TableServiceClientFactory.Instance.Add<T>(options);
            settings.Services.AddSingleton(TableServiceClientFactory.Instance);
            settings.SetStorage<TableStorageRepository<T, TKey>>(type, ServiceLifetime.Singleton);
            return new RepositoryTableStorageBuilder<T, TKey>(settings.Services)
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T, TKey>>();
        }
        /// <summary>
        /// Add a default table storage service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddRepositoryInTableStorage<T, TKey>(
           this IRepositorySettings<T, TKey> settings,
           Action<TableStorageConnectionSettings> connectionSettings)
            where TKey : notnull
             => settings.AddRepositoryInTableStorage(PatternType.Repository, connectionSettings);
        /// <summary>
        /// Add a default table storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddCommandInTableStorage<T, TKey>(
           this IRepositorySettings<T, TKey> settings,
           Action<TableStorageConnectionSettings> connectionSettings)
            where TKey : notnull
             => settings.AddRepositoryInTableStorage(PatternType.Command, connectionSettings);
        /// <summary>
        /// Add a default table storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddQueryInTableStorage<T, TKey>(
           this IRepositorySettings<T, TKey> settings,
           Action<TableStorageConnectionSettings> connectionSettings)
            where TKey : notnull
             => settings.AddRepositoryInTableStorage(PatternType.Query, connectionSettings);
    }
}
