using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.InMemory.Population;

namespace RepositoryFramework.Customization
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Override the population strategy default service.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TService">your IPopulationService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPopulationStrategyService<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IPopulationStrategy<T, TKey>
          where TKey : notnull
          => services.AddSingleton<IPopulationStrategy<T, TKey>, TService>();
        /// <summary>
        /// Override the population default service.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TService">your IPopulationService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPopulationService<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IPopulationService
          where TKey : notnull
          => services.AddSingleton<IPopulationService, TService>();
        /// <summary>
        /// Override the default instance creator for you population service.
        /// </summary>
        /// <typeparam name="T">your IInstanceCreator</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddInstanceCreatorServiceForPopulation<T>(
            this IServiceCollection services)
            where T : class, IInstanceCreator
            => services.AddSingleton<IInstanceCreator, T>();
        /// <summary>
        /// Override the default regular expression service for you population service.
        /// </summary>
        /// <typeparam name="T">your IRegexService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRegexService<T>(
            this IServiceCollection services)
            where T : class, IRegexService
            => services.AddSingleton<IRegexService, T>();
        /// <summary>
        /// Add a random population service to your population service, you can use Priority property to override default behavior.
        /// For example a service for string random generation already exists with Priority 1,
        /// you may create another string random service with Priority = 2 or greater of 1.
        /// In IsValid method you have to check if type is a string to complete the override.
        /// </summary>
        /// <typeparam name="T">your IRandomPopulationService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRandomPopulationService<T>(this IServiceCollection services,
            IRandomPopulationService service)
        {
            PopulationServiceSelector.Instance.TryAdd(service);
            services.AddSingleton(PopulationServiceSelector.Instance);
            return services;
        }
    }
}
