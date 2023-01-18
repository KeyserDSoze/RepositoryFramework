﻿namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static bool s_throwExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes = false;
        public static IServiceCollection ThrowExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes(this IServiceCollection services)
        {
            s_throwExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes = true;
            return services;
        }
        public static IServiceCollection AddService<TImplementation>(this IServiceCollection services,
           ServiceLifetime lifetime)
           where TImplementation : class
           => lifetime switch
           {
               ServiceLifetime.Transient => services.AddTransient<TImplementation>(),
               ServiceLifetime.Singleton => services.AddSingleton<TImplementation>(),
               _ => services.AddScoped<TImplementation>()
           };
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
        public static IServiceCollection AddService(this IServiceCollection services,
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime)
           => lifetime switch
           {
               ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
               ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
               _ => services.AddScoped(serviceType, implementationType)
           };

        public static IServiceCollection RemoveServiceIfAlreadyInstalled<TStorage>(this IServiceCollection services,
            params Type[] types)
        {
            foreach (var type in types)
            {
                var serviceDescriptors = services.Where(descriptor => descriptor.ServiceType == type).ToList();
                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    if (s_throwExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes)
                        throw new ArgumentException($"You have two configurations of the same interface {serviceDescriptor.ServiceType.FullName}. {typeof(TStorage).FullName} wants to override {serviceDescriptor.ImplementationType?.FullName} with lifetime {serviceDescriptor.Lifetime}.");
                    services.Remove(serviceDescriptor);
                }
            }
            return services;
        }
        /// <summary>
        /// Add an action after the build of your Service Collection, you have to call WarmUpAsync method in your built IServiceProvider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="actionAfterBuild"></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddEventAfterServiceCollectionBuild(this IServiceCollection services,
            Func<IServiceProvider, Task> actionAfterBuild)
            => services.AddWarmUp(actionAfterBuild);
    }
}
