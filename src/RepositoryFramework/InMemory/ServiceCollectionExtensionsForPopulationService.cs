using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Population;
using RepositoryFramework.Services;

namespace RepositoryFramework.Customization
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPopulationService<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IPopulationService<T, TKey>
          where TKey : notnull
          => services.AddSingleton<IPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddPopulationServiceFactory<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IPopulationServiceFactory<T, TKey>
          where TKey : notnull
          => services.AddSingleton<IPopulationServiceFactory<T, TKey>, TService>();
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
            where TService : class, IAbstractPopulationService<T, TKey>
            where TKey : notnull
            => services.AddSingleton<IAbstractPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddArrayServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IArrayPopulationService<T, TKey>
            where TKey : notnull
            => services.AddSingleton<IArrayPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddBoolServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IBoolPopulationService<T, TKey>
            where TKey : notnull
            => services.AddSingleton<IBoolPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddByteServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IBytePopulationService<T, TKey>
            where TKey : notnull
            => services.AddSingleton<IBytePopulationService<T, TKey>, TService>();
        public static IServiceCollection AddCharServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, ICharPopulationService<T, TKey>
            where TKey : notnull
            => services.AddSingleton<ICharPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddClassServiceForPopulation<T, TKey, TService>(
            this IServiceCollection services)
            where TService : class, IClassPopulationService<T, TKey>
            where TKey : notnull
            => services.AddSingleton<IClassPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddDeletagedServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IDelegatedPopulationService<T, TKey>
           where TKey : notnull
           => services.AddSingleton<IDelegatedPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddDictionaryServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IDictionaryPopulationService<T, TKey>
           where TKey : notnull
           => services.AddSingleton<IDictionaryPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddEnumerableServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IEnumerablePopulationService<T, TKey>
           where TKey : notnull
           => services.AddSingleton<IEnumerablePopulationService<T, TKey>, TService>();
        public static IServiceCollection AddGuidServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IGuidPopulationService<T, TKey>
           where TKey : notnull
           => services.AddSingleton<IGuidPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddConcretizationServiceForPopulation<T, TKey, TService>(
           this IServiceCollection services)
           where TService : class, IConcretizationPopulationService<T, TKey>
           where TKey : notnull
           => services.AddSingleton<IConcretizationPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddNumberServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, INumberPopulationService<T, TKey>
          where TKey : notnull
          => services.AddSingleton<INumberPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddRangeServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IRangePopulationService<T, TKey>
          where TKey : notnull
          => services.AddSingleton<IRangePopulationService<T, TKey>, TService>();
        public static IServiceCollection AddRegexServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IRegexPopulationService<T, TKey>
          where TKey : notnull
          => services.AddSingleton<IRegexPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddStringServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, IStringPopulationService<T, TKey>
          where TKey : notnull
          => services.AddSingleton<IStringPopulationService<T, TKey>, TService>();
        public static IServiceCollection AddTimeServiceForPopulation<T, TKey, TService>(
          this IServiceCollection services)
          where TService : class, ITimePopulationService<T, TKey>
          where TKey : notnull
          => services.AddSingleton<ITimePopulationService<T, TKey>, TService>();
    }
}