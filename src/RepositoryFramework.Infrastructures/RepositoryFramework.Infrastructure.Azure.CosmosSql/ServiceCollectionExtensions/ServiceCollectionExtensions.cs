using Microsoft.Azure.Cosmos;
using RepositoryFramework.Infrastructure.Azure.CosmosSql;
using System.Linq.Expressions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryInCosmosSql<T, TKey>(
           this IServiceCollection services,
           Expression<Func<T, TKey>> navigationKey,
           string connectionString,
           string databaseName,
           string? containerName = null,
           CosmosClientOptions? clientOptions = null,
           CosmosOptions? databaseOptions = null,
           CosmosOptions? containerOptions = null)
            where TKey : notnull
        {
            CosmosSqlServiceClientFactory.Instance.Add<T>(databaseName, containerName ?? typeof(T).Name, navigationKey.ToString().Split('.').Last(),
                connectionString,
                clientOptions,
                databaseOptions,
                containerOptions);
            services.AddSingleton(CosmosSqlServiceClientFactory.Instance);
            services.AddRepository<T, TKey, CosmosSqlRepository<T, TKey>>(ServiceLifetime.Singleton);
            return services;
        }
    }
}
