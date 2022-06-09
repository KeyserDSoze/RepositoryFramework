using Azure.Storage.Blobs;
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
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryInBlobStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           BlobClientOptions? clientOptions = null)
            where TKey : notnull
        {
            BlobServiceClientFactory.Instance.Add(name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            services.AddRepository<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
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
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommandInBlobStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           BlobClientOptions? clientOptions = null)
            where TKey : notnull
        {
            BlobServiceClientFactory.Instance.Add(name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            services.AddCommand<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
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
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddQueryInBlobStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           BlobClientOptions? clientOptions = null)
            where TKey : notnull
        {
            BlobServiceClientFactory.Instance.Add(name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            services.AddQuery<T, TKey, BlobStorageRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
    }
}