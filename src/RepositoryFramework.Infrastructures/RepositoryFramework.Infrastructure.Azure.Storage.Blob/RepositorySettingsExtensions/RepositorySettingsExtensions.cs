using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Storage.Blob;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositorySettingsExtensions
    {
        private static IRepositoryBlobStorageBuilder<T, TKey> WithBlobStorage<T, TKey>(
          this IRepositorySettings<T, TKey> settings,
          PatternType type,
          Action<BlobStorageConnectionSettings> connectionSettings)
           where TKey : notnull
        {
            var options = new BlobStorageConnectionSettings();
            connectionSettings.Invoke(options);
            BlobServiceClientFactory.Instance.Add<T>(options);
            settings.Services.AddSingleton(BlobServiceClientFactory.Instance);
            settings.SetStorage<BlobStorageRepository<T, TKey>>(type, ServiceLifetime.Singleton);
            return new RepositoryBlobStorageBuilder<T, TKey>(settings.Services);
        }
        /// <summary>
        /// Add a default blob storage service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <returns>IRepositoryBlobStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBlobStorageBuilder<T, TKey> WithBlobStorage<T, TKey>(
          this IRepositorySettings<T, TKey> settings,
          Action<BlobStorageConnectionSettings> connectionSettings)
            where TKey : notnull
            => settings.WithBlobStorage(PatternType.Repository, connectionSettings);
        /// <summary>
        /// Add a default blob storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <returns>IRepositoryBlobStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBlobStorageBuilder<T, TKey> WithCommandInBlobStorage<T, TKey>(
           this IRepositorySettings<T, TKey> settings,
           Action<BlobStorageConnectionSettings> connectionSettings)
               where TKey : notnull
               => settings.WithBlobStorage(PatternType.Command, connectionSettings);
        /// <summary>
        /// Add a default blob storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="connectionSettings">Settings for your storage connection.</param>
        /// <returns>IRepositoryBlobStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBlobStorageBuilder<T, TKey> WithQueryInBlobStorage<T, TKey>(
           this IRepositorySettings<T, TKey> settings,
           Action<BlobStorageConnectionSettings> connectionSettings)
            where TKey : notnull
               => settings.WithBlobStorage(PatternType.Query, connectionSettings);
    }
}
