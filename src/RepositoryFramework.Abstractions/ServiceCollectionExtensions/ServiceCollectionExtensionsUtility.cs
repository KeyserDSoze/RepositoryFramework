using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static RepositoryFrameworkService SetService<T>(this IServiceCollection services)
            => services.SetService<T, string, bool>();
        private static RepositoryFrameworkService SetService<T, TKey>(this IServiceCollection services)
            where TKey : notnull
            => services.SetService<T, TKey, bool>();
        private static RepositoryFrameworkService SetService<T, TKey, TState>(this IServiceCollection services)
            where TKey : notnull
        {
            Type entityType = typeof(T);
            Type keyType = typeof(TKey);
            Type stateType = typeof(TState);
            var service = RepositoryFrameworkRegistry.Instance.Services.FirstOrDefault(x => x.ModelType == entityType);
            if (service == null)
            {
                service = new RepositoryFrameworkService { ModelType = entityType };
                RepositoryFrameworkRegistry.Instance.Services.Add(service);
                services.AddSingleton(RepositoryFrameworkRegistry.Instance);
            }
            service.KeyType = keyType;
            service.StateType = stateType;
            return service;
        }
        public static IServiceCollection AddService<TService, TImplementation>(this IServiceCollection services,
            ServiceLifetime lifetime)
            where TImplementation : class, TService
            where TService : class
            => lifetime switch
            {
                ServiceLifetime.Transient => services.AddTransient<TService, TImplementation>(),
                ServiceLifetime.Singleton => services.AddSingleton<TService, TImplementation>(),
                _ => services.AddScoped<TService, TImplementation>()
            };
    }
}