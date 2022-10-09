﻿using RepositoryFramework;
using RepositoryFramework.Cache;
using RepositoryFramework.Cache.Azure.Storage.Blob;
using RepositoryFramework.Infrastructure.Azure.Storage.Blob;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add Azure Blob Storage cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your storage connection.</param>
        /// <param name="settings">Settings for your cache.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithBlobStorageCache<T, TKey>(
           this IRepositoryBuilder<T, TKey> builder,
                Action<BlobStorageConnectionSettings> options,
                Action<DistributedCacheOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            builder.Services
                  .AddRepositoryInBlobStorage<BlobStorageCacheModel, string>(options);
            return builder.WithDistributedCache<T, TKey, BlobStorageCache<T, TKey>>(settings, ServiceLifetime.Singleton);
        }
    }
}
