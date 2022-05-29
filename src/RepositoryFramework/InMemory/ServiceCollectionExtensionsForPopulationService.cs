using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Population;
using RepositoryFramework.Services;

namespace RepositoryFramework.Customization
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPopulationService<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IPopulationService
          where TKey : notnull
          => services.AddSingleton<IPopulationService, TService>();
        public static IServiceCollection AddInstanceCreatorServiceForPopulation<T>(
            this IServiceCollection services)
            where T : class, IInstanceCreator
            => services.AddSingleton<IInstanceCreator, T>();
        public static IServiceCollection AddRegexService<T>(
            this IServiceCollection services)
            where T : class, IRegexService
            => services.AddSingleton<IRegexService, T>();
        public static IServiceCollection AddRandomPopulationService(this IServiceCollection services,
            IRandomPopulationService service)
        {
            PopulationServiceSelector.Instance.TryAdd(service);
            services.AddSingleton(PopulationServiceSelector.Instance);
            return services;
        }
    }
}