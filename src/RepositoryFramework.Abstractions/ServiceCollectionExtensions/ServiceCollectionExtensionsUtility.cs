using Microsoft.Extensions.DependencyInjection.Extensions;
using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static bool _throwExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes = false;
        public static IServiceCollection ThrowExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes(this IServiceCollection services)
        {
            _throwExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes = true;
            return services;
        }
        private static RepositoryFrameworkService SetService<T>(this IServiceCollection services)
            => services.SetService<T, string, State<T>>();
        private static RepositoryFrameworkService SetService<T, TKey>(this IServiceCollection services)
            where TKey : notnull
            => services.SetService<T, TKey, State<T>>();
        private static RepositoryFrameworkService SetService<T, TKey, TState>(this IServiceCollection services)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            Type entityType = typeof(T);
            Type keyType = typeof(TKey);
            Type stateType = typeof(TState);
            var service = RepositoryFrameworkRegistry.Instance.Services.FirstOrDefault(x => x.ModelType == entityType);
            if (service == null)
            {
                service = new RepositoryFrameworkService { ModelType = entityType };
                RepositoryFrameworkRegistry.Instance.Services.Add(service);
                services.TryAddSingleton(RepositoryFrameworkRegistry.Instance);
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

        private static IServiceCollection RemoveServiceIfAlreadyInstalled<TStorage>(this IServiceCollection services,
            params Type[] types)
        {
            foreach (var type in types)
            {
                var serviceDescriptors = services.Where(descriptor => descriptor.ServiceType == type).ToList();
                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    if (_throwExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes)
                        throw new ArgumentException($"You have two configurations of the same interface {serviceDescriptor.ServiceType.FullName}. {typeof(TStorage).FullName} wants to override {serviceDescriptor.ImplementationType?.FullName} with lifetime {serviceDescriptor.Lifetime}.");
                    services.Remove(serviceDescriptor);
                }
            }
            return services;
        }
    }
}