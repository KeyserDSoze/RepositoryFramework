﻿using Azure.Storage.Blobs;
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
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="isInvisibleForApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> AddRepositoryInBlobStorage<T>(
           this IServiceCollection services,
           Uri endpointUri,
           BlobClientOptions? clientOptions = null,
           bool isInvisibleForApi = false)
        {
            BlobServiceClientFactory.Instance.Add(typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddRepository<T, BlobStorageRepository<T>>(ServiceLifetime.Singleton, isInvisibleForApi);
        }
        /// <summary>
        /// Add a default blob storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="isInvisibleForApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> AddCommandInBlobStorage<T>(
           this IServiceCollection services,
           Uri endpointUri,
           BlobClientOptions? clientOptions = null,
           bool isInvisibleForApi = false)
        {
            BlobServiceClientFactory.Instance.Add(typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddCommand<T, BlobStorageRepository<T>>(ServiceLifetime.Singleton, isInvisibleForApi);
        }
        /// <summary>
        /// Add a default blob storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="clientOptions">Options to configure the requests to the Blob service.</param>
        /// <param name="isInvisibleForApi">It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> AddQueryInBlobStorage<T>(
           this IServiceCollection services,
           Uri endpointUri,
           BlobClientOptions? clientOptions = null,
           bool isInvisibleForApi = false)
        {
            BlobServiceClientFactory.Instance.Add(typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(BlobServiceClientFactory.Instance);
            return services.AddQuery<T, BlobStorageRepository<T>>(ServiceLifetime.Singleton, isInvisibleForApi);
        }
    }
}