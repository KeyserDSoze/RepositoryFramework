﻿using System.Linq.Expressions;
using Microsoft.Azure.Cosmos;
using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Cosmos.Sql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default cosmos sql service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="navigationKey">Select the property that represents your key.</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="databaseName">Name for your database, it will be created automatically if not exists.</param>
        /// <param name="containerName">Name for your container, if you omit it the name will be the model name,
        ///     it will be created automatically if not exists.</param>
        /// <param name="clientOptions">Options for cosmos db client.</param>
        /// <param name="databaseOptions">Options for cosmos database.</param>
        /// <param name="containerOptions">Options for cosmos container.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> AddRepositoryInCosmosSql<T, TKey>(
           this IServiceCollection services,
           string connectionString,
           string databaseName,
           string? containerName = null,
           CosmosClientOptions? clientOptions = null,
           CosmosOptions? databaseOptions = null,
           CosmosOptions? containerOptions = null,
           Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            CosmosSqlServiceClientFactory.Instance
                .Add<T>(databaseName,
                        containerName ?? typeof(T).Name,
                        "id",
                        connectionString,
                        clientOptions,
                        databaseOptions,
                        containerOptions);
            services.AddSingleton(new CosmosOptions<T, TKey>(containerName ?? typeof(T).Name));
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            return new RepositoryCosmosSqlBuilder<T, TKey>(services.AddRepository<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
        /// <summary>
        /// Add a default cosmos sql service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="navigationKey">Select the property that represents your key.</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="databaseName">Name for your database, it will be created automatically if not exists.</param>
        /// <param name="containerName">Name for your container, if you omit it the name will be the model name,
        //     it will be created automatically if not exists.</param>
        /// <param name="clientOptions">Options for cosmos db client.</param>
        /// <param name="databaseOptions">Options for cosmos database.</param>
        /// <param name="containerOptions">Options for cosmos container.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> AddCommandInCosmosSql<T, TKey>(
           this IServiceCollection services,
           Expression<Func<T, TKey>> navigationKey,
           string connectionString,
           string databaseName,
           string? containerName = null,
           CosmosClientOptions? clientOptions = null,
           CosmosOptions? databaseOptions = null,
           CosmosOptions? containerOptions = null,
           Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            CosmosSqlServiceClientFactory.Instance
                .Add<T>(databaseName,
                        containerName ?? typeof(T).Name,
                        navigationKey.ToString().Split('.').Last(),
                        connectionString,
                        clientOptions,
                        databaseOptions,
                        containerOptions);
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            return new RepositoryCosmosSqlBuilder<T, TKey>(services.AddCommand<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
        /// <summary>
        /// Add a default cosmos sql service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="navigationKey">Select the property that represents your key.</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="databaseName">Name for your database, it will be created automatically if not exists.</param>
        /// <param name="containerName">Name for your container, if you omit it the name will be the model name,
        ///     it will be created automatically if not exists.</param>
        /// <param name="clientOptions">Options for cosmos db client.</param>
        /// <param name="databaseOptions">Options for cosmos database.</param>
        /// <param name="containerOptions">Options for cosmos container.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryCosmosSqlBuilder<T, TKey> AddQueryInCosmosSql<T, TKey>(
           this IServiceCollection services,
           Expression<Func<T, TKey>> navigationKey,
           string connectionString,
           string databaseName,
           string? containerName = null,
           CosmosClientOptions? clientOptions = null,
           CosmosOptions? databaseOptions = null,
           CosmosOptions? containerOptions = null,
           Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            CosmosSqlServiceClientFactory.Instance
                .Add<T>(databaseName,
                        containerName ?? typeof(T).Name,
                        navigationKey.ToString().Split('.').Last(),
                        connectionString,
                        clientOptions,
                        databaseOptions,
                        containerOptions);
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            return new RepositoryCosmosSqlBuilder<T, TKey>(services.AddQuery<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
    }
}
