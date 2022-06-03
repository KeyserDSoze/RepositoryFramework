﻿using Microsoft.Azure.Cosmos;
using RepositoryFramework.Infrastructure.Azure.CosmosSql;
using System.Linq.Expressions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default cosmos sql service for your repository pattern
        // with default credential integration (managed identity)
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="navigationKey">Select the property that represents your key.</param>
        /// <param name="endpointUri">Uri of your cosmos db.</param>
        /// <param name="databaseName">Name for your database, it will be created automatically if not exists.</param>
        /// <param name="containerName">Name for your container, if you omit it the name will be the model name,
        ///     it will be created automatically if not exists.</param>
        /// <param name="clientOptions">Options for cosmos db client.</param>
        /// <param name="databaseOptions">Options for cosmos database.</param>
        /// <param name="containerOptions">Options for cosmos container.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryInCosmosSql<T, TKey>(
           this IServiceCollection services,
           Expression<Func<T, TKey>> navigationKey,
           Uri endpointUri,
           string databaseName,
           string? containerName = null,
           CosmosClientOptions? clientOptions = null,
           CosmosOptions? databaseOptions = null,
           CosmosOptions? containerOptions = null)
            where TKey : notnull
        {
            CosmosSqlServiceClientFactory.Instance
                .Add<T>(databaseName,
                        containerName ?? typeof(T).Name,
                        navigationKey.ToString().Split('.').Last(),
                        endpointUri,
                        clientOptions,
                        databaseOptions,
                        containerOptions);
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            services.AddRepository<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
        /// <summary>
        /// Add a default cosmos sql service for your command pattern.
        /// with default credential integration (managed identity)
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="navigationKey">Select the property that represents your key.</param>
        /// <param name="endpointUri">Uri of your cosmos db.</param>
        /// <param name="databaseName">Name for your database, it will be created automatically if not exists.</param>
        /// <param name="containerName">Name for your container, if you omit it the name will be the model name,
        ///     it will be created automatically if not exists.</param>
        /// <param name="clientOptions">Options for cosmos db client.</param>
        /// <param name="databaseOptions">Options for cosmos database.</param>
        /// <param name="containerOptions">Options for cosmos container.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommandInCosmosSql<T, TKey>(
           this IServiceCollection services,
           Expression<Func<T, TKey>> navigationKey,
           Uri endpointUri,
           string databaseName,
           string? containerName = null,
           CosmosClientOptions? clientOptions = null,
           CosmosOptions? databaseOptions = null,
           CosmosOptions? containerOptions = null)
            where TKey : notnull
        {
            CosmosSqlServiceClientFactory.Instance
                .Add<T>(databaseName,
                        containerName ?? typeof(T).Name,
                        navigationKey.ToString().Split('.').Last(),
                        endpointUri,
                        clientOptions,
                        databaseOptions,
                        containerOptions);
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            services.AddCommand<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
        /// <summary>
        /// Add a default cosmos sql service for your query pattern.
        /// with default credential integration (managed identity)
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="navigationKey">Select the property that represents your key.</param>
        /// <param name="endpointUri">Uri of your cosmos db.</param>
        /// <param name="databaseName">Name for your database, it will be created automatically if not exists.</param>
        /// <param name="containerName">Name for your container, if you omit it the name will be the model name,
        ///     it will be created automatically if not exists.</param>
        /// <param name="clientOptions">Options for cosmos db client.</param>
        /// <param name="databaseOptions">Options for cosmos database.</param>
        /// <param name="containerOptions">Options for cosmos container.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddQueryInCosmosSql<T, TKey>(
           this IServiceCollection services,
           Expression<Func<T, TKey>> navigationKey,
           Uri endpointUri,
           string databaseName,
           string? containerName = null,
           CosmosClientOptions? clientOptions = null,
           CosmosOptions? databaseOptions = null,
           CosmosOptions? containerOptions = null)
            where TKey : notnull
        {
            CosmosSqlServiceClientFactory.Instance
                .Add<T>(databaseName,
                        containerName ?? typeof(T).Name,
                        navigationKey.ToString().Split('.').Last(),
                        endpointUri,
                        clientOptions,
                        databaseOptions,
                        containerOptions);
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            services.AddQuery<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
    }
}