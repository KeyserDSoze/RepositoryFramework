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
        public static IServiceCollection AddPopulationServiceFactory<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IPopulationServiceFactory
          where TKey : notnull
          => services.AddSingleton<IPopulationServiceFactory, TService>();
        public static IServiceCollection AddInstanceCreatorServiceForPopulation<T>(
            this IServiceCollection services)
            where T : class, IInstanceCreator
            => services.AddSingleton<IInstanceCreator, T>();
        public static IServiceCollection AddRegexService<T>(
            this IServiceCollection services)
            where T : class, IRegexService
            => services.AddSingleton<IRegexService, T>();
        public static IServiceCollection AddAbstractServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IAbstractPopulationService
            where TKey : notnull
            => services.AddSingleton<IAbstractPopulationService, TService>();
        public static IServiceCollection AddArrayServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IArrayPopulationService
            where TKey : notnull
            => services.AddSingleton<IArrayPopulationService, TService>();
        public static IServiceCollection AddBoolServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IBoolPopulationService
            where TKey : notnull
            => services.AddSingleton<IBoolPopulationService, TService>();
        public static IServiceCollection AddByteServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IBytePopulationService
            where TKey : notnull
            => services.AddSingleton<IBytePopulationService, TService>();
        public static IServiceCollection AddCharServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, ICharPopulationService
            where TKey : notnull
            => services.AddSingleton<ICharPopulationService, TService>();
        public static IServiceCollection AddClassServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IClassPopulationService
            where TKey : notnull
            => services.AddSingleton<IClassPopulationService, TService>();
        public static IServiceCollection AddDeletagedServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IDelegatedPopulationService
           where TKey : notnull
           => services.AddSingleton<IDelegatedPopulationService, TService>();
        public static IServiceCollection AddDictionaryServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IDictionaryPopulationService
           where TKey : notnull
           => services.AddSingleton<IDictionaryPopulationService, TService>();
        public static IServiceCollection AddEnumerableServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IEnumerablePopulationService
           where TKey : notnull
           => services.AddSingleton<IEnumerablePopulationService, TService>();
        public static IServiceCollection AddGuidServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IGuidPopulationService
           where TKey : notnull
           => services.AddSingleton<IGuidPopulationService, TService>();
        public static IServiceCollection AddConcretizationServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IConcretizationPopulationService
           where TKey : notnull
           => services.AddSingleton<IConcretizationPopulationService, TService>();
        public static IServiceCollection AddNumberServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, INumberPopulationService
          where TKey : notnull
          => services.AddSingleton<INumberPopulationService, TService>();
        public static IServiceCollection AddRangeServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IRangePopulationService
          where TKey : notnull
          => services.AddSingleton<IRangePopulationService, TService>();
        public static IServiceCollection AddRegexServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IRegexPopulationService
          where TKey : notnull
          => services.AddSingleton<IRegexPopulationService, TService>();
        public static IServiceCollection AddStringServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IStringPopulationService
          where TKey : notnull
          => services.AddSingleton<IStringPopulationService, TService>();
        public static IServiceCollection AddTimeServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, ITimePopulationService
          where TKey : notnull
          => services.AddSingleton<ITimePopulationService, TService>();
    }
}