using Azure.Data.Tables;
using RepositoryFramework.Infrastructure.Azure.Storage.Table;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default table storage service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Table service.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddRepositoryInTableStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           TableClientOptions? clientOptions = null)
            where TKey : notnull
        {
            TableServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T, TKey>(services.AddRepository<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T, TKey>>();
        }
        /// <summary>
        /// Add a default table storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Table service.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddCommandInTableStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           TableClientOptions? clientOptions = null)
            where TKey : notnull
        {
            TableServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T, TKey>(services.AddCommand<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T, TKey>>();
        }
        /// <summary>
        /// Add a default table storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="endpointUri">Uri of your storage.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Table service.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryTableStorageBuilder<T, TKey> AddQueryInTableStorage<T, TKey>(
           this IServiceCollection services,
           Uri endpointUri,
           string? name = null,
           TableClientOptions? clientOptions = null)
            where TKey : notnull
        {
            TableServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, endpointUri, clientOptions);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T, TKey>(services.AddQuery<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T, TKey>>();
        }
    }
}