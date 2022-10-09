using Azure.Storage.Blobs;
using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Storage.Blob;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default blob storage service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddRepositoryInBlobStorage<T, TKey>(
           this IServiceCollection services,
                Action<BlobStorageConnectionSettings> connectionSettings,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new BlobStorageConnectionSettings();
            connectionSettings.Invoke(options);
            BlobServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddRepository<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton, settings);
        }
        /// <summary>
        /// Add a default blob storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddCommandInBlobStorage<T, TKey>(
           this IServiceCollection services,
             Action<BlobStorageConnectionSettings> connectionSettings,
             Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new BlobStorageConnectionSettings();
            connectionSettings.Invoke(options);
            BlobServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddCommand<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton, settings);
        }
        /// <summary>
        /// Add a default blob storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddQueryInBlobStorage<T, TKey>(
           this IServiceCollection services,
           Action<BlobStorageConnectionSettings> connectionSettings,
           Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            var options = new BlobStorageConnectionSettings();
            connectionSettings.Invoke(options);
            BlobServiceClientFactory.Instance.Add<T>(options);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddQuery<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton, settings);
        }
    }
}
