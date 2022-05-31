using RepositoryFramework.Infrastructure.TableStorage;
using RepositoryFramework.Infrastructure.TableStorage.TableStorage;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
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
    }
}
