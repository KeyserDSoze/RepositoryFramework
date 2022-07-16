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
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your container, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="settings">Settings for your cache.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static RepositoryBuilder<T, TKey, TState> WithBlobStorageCache<T, TKey, TState>(
           this RepositoryBuilder<T, TKey, TState> builder,
           string connectionString,
           string? name = null,
           BlobClientOptions? clientOptions = null,
           Action<DistributedCacheOptions<T, TKey, TState>>? settings = null)
            where TKey : notnull
            where TState : IState
        {
            builder.Services
                  .AddRepositoryInBlobStorage<BlobStorageCacheModel, string>(connectionString, name, clientOptions, true);
            return builder.WithDistributedCache<T, TKey, TState, BlobStorageCache<T, TKey, TState>>(settings, ServiceLifetime.Singleton);
        }
        /// <summary>
        /// Add Azure Blob Storage cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="settings">Settings for your cache.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static RepositoryBuilder<T, TKey, TState> WithBlobStorageCache<T, TKey, TState>(
           this RepositoryBuilder<T, TKey, TState> builder,
           Uri endpointUri,
           BlobClientOptions? clientOptions = null,
           Action<DistributedCacheOptions<T, TKey, TState>>? settings = null)
            where TKey : notnull
            where TState : IState
        {
            builder.Services
                  .AddRepositoryInBlobStorage<BlobStorageCacheModel, string>(endpointUri, clientOptions, true);
            return builder.WithDistributedCache<T, TKey, TState, BlobStorageCache<T, TKey, TState>>(settings, ServiceLifetime.Singleton);
        }
    }
}
