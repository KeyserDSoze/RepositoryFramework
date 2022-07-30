using Azure.Storage.Blobs;
using RepositoryFramework;
using RepositoryFramework.Cache;
using RepositoryFramework.Cache.Azure.Storage.Blob;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add Azure Blob Storage cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your container, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="settings">Settings for your cache.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithBlobStorageCache<T>(
           this IRepositoryBuilder<T> builder,
           string connectionString,
           string? name = null,
           BlobClientOptions? clientOptions = null,
           Action<DistributedCacheOptions<T, string, State<T>>>? settings = null)
        {
            builder.Services
                   .AddRepositoryInBlobStorage<BlobStorageCacheModel, string>(connectionString, name, clientOptions, true);
            return builder.WithDistributedCache<T, BlobStorageCache<T>>(settings, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Add Azure Blob Storage cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="settings">Settings for your cache.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithBlobStorageCache<T>(
           this IRepositoryBuilder<T> builder,
           Uri endpointUri,
           BlobClientOptions? clientOptions = null,
           Action<DistributedCacheOptions<T, string, State<T>>>? settings = null)
        {
            builder.Services
                   .AddRepositoryInBlobStorage<BlobStorageCacheModel, string>(endpointUri, clientOptions, true);
            return builder.WithDistributedCache<T, BlobStorageCache<T>>(settings, ServiceLifetime.Singleton);
        }
    }
}
