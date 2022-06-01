using RepositoryFramework.Infrastructure.Azure.TableStorage;

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
        /// <param name="connectionString">A connection string includes the authentication information required for your
        //     application to access data in an Azure Storage account at runtime. For more information,
        //     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryInTableStorage<T, TKey>(
           this IServiceCollection services,
           string connectionString,
           string? name = null)
            where TKey : notnull
        {
            TableServiceClientFactory.Instance.Add(name ?? typeof(T).Name, connectionString);
            services.AddSingleton(TableServiceClientFactory.Instance);
            services.AddRepository<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
        /// <summary>
        /// Add a default table storage service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        //     application to access data in an Azure Storage account at runtime. For more information,
        //     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommandInTableStorage<T, TKey>(
           this IServiceCollection services,
           string connectionString,
           string? name = null)
            where TKey : notnull
        {
            TableServiceClientFactory.Instance.Add(name ?? typeof(T).Name, connectionString);
            services.AddSingleton(TableServiceClientFactory.Instance);
            services.AddCommand<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
        /// <summary>
        /// Add a default table storage service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="connectionString">A connection string includes the authentication information required for your
        //     application to access data in an Azure Storage account at runtime. For more information,
        //     Configure Azure Storage connection strings.</param>
        /// <param name="name">Optional name for your table, if you omit it, the service will use the name of your model.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddQueryInTableStorage<T, TKey>(
           this IServiceCollection services,
           string connectionString,
           string? name = null)
            where TKey : notnull
        {
            TableServiceClientFactory.Instance.Add(name ?? typeof(T).Name, connectionString);
            services.AddSingleton(TableServiceClientFactory.Instance);
            services.AddQuery<T, TKey, TableStorageRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
    }
}