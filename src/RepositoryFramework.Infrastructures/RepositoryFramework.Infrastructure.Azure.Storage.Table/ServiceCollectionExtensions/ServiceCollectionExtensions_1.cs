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
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Table service.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryTableStorageBuilder<T> AddRepositoryInTableStorage<T>(
           this IServiceCollection services,
           string connectionString,
           string? name = null,
           TableClientOptions? clientOptions = null)
        {
            TableServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, connectionString, clientOptions);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T>(services.AddRepository<T, TableStorageRepository<T>>(ServiceLifetime.Singleton))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T>>();
        }
        /// <summary>
        /// Add a default table storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Table service.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryTableStorageBuilder<T> AddCommandInTableStorage<T>(
           this IServiceCollection services,
           string connectionString,
           string? name = null,
           TableClientOptions? clientOptions = null)
        {
            TableServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, connectionString, clientOptions);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T>(services.AddCommand<T, TableStorageRepository<T>>(ServiceLifetime.Singleton))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T>>();
        }
        /// <summary>
        /// Add a default table storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        ///     application to access data in an Azure Storage account at runtime. For more information,
        ///     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <param name="clientOptions">Options to configure the requests to the Table service.</param>
        /// <returns>IRepositoryTableStorageBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryTableStorageBuilder<T> AddQueryInTableStorage<T>(
           this IServiceCollection services,
           string connectionString,
           string? name = null,
           TableClientOptions? clientOptions = null)
        {
            TableServiceClientFactory.Instance.Add(typeof(T).Name, name ?? typeof(T).Name, connectionString, clientOptions);
            services.AddSingleton(TableServiceClientFactory.Instance);
            return new RepositoryTableStorageBuilder<T>(services.AddQuery<T, TableStorageRepository<T>>(ServiceLifetime.Singleton))
                .WithTableStorageKeyReader<DefaultTableStorageKeyReader<T>>();
        }
    }
}