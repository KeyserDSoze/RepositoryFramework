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
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="name">Optional name for your container, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="isPrivate">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T, TKey> AddRepositoryInBlobStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           BlobClientOptions? clientOptions = null,
           bool isPrivate = false)
            where TKey : notnull
        {
            BlobServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddRepository<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton, isPrivate);
        }
        /// <summary>
        /// Add a default blob storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="name">Optional name for your container, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="isPrivate">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T, TKey> AddCommandInBlobStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           BlobClientOptions? clientOptions = null,
           bool isPrivate = false)
            where TKey : notnull
        {
            BlobServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddCommand<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton, isPrivate);
        }
        /// <summary>
        /// Add a default blob storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="name">Optional name for your container, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="isPrivate">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T, TKey> AddQueryInBlobStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           BlobClientOptions? clientOptions = null,
           bool isPrivate = false)
            where TKey : notnull
        {
            BlobServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddQuery<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton, isPrivate);
        }
    }
}