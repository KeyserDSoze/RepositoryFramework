using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Storage.Table;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default table storage service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddRepositoryInTableStorage<T, TKey>(
           this IServiceCollection services,
           Action<TableStorageConnectionSettings> connectionSettings,
           Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new TableStorageConnectionSettings();
            connectionSettings.Invoke(options);
            TableServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T, TKey>(services.AddRepository<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton, settings))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T, TKey>>();
        }
        /// <summary>
        /// Add a default table storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddCommandInTableStorage<T, TKey>(
           this IServiceCollection services,
           Action<TableStorageConnectionSettings> connectionSettings,
           Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new TableStorageConnectionSettings();
            connectionSettings.Invoke(options);
            TableServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T, TKey>(services.AddCommand<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton, settings))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T, TKey>>();
        }
        /// <summary>
        /// Add a default table storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddQueryInTableStorage<T, TKey>(
           this IServiceCollection services,
           Action<TableStorageConnectionSettings> connectionSettings,
           Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new TableStorageConnectionSettings();
            connectionSettings.Invoke(options);
            TableServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T, TKey>(services
                .AddQuery<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton, settings))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T, TKey>>();
        }
    }
}
