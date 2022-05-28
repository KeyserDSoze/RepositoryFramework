using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Population;
using RepositoryFramework.Services;
using System.Linq.Expressions;

namespace RepositoryFramework
{
    public class RepositoryPatternInMemoryBuilder<T, TKey>
        where TKey : notnull
    {
        private readonly IServiceCollection _services;
        private readonly InternalBehaviorSettings _internalBehaviorSettings = new();
        public RepositoryPatternInMemoryBuilder(IServiceCollection services)
            => _services = services;
        public RepositoryPatternInMemoryBuilder<TNext, TNextKey> AddRepositoryPatternInMemoryStorage<TNext, TNextKey>(Action<RepositoryPatternBehaviorSettings<TNext, TNextKey>>? settings = default)
            where TNextKey : notnull
            => _services!.AddRepositoryPatternInMemoryStorage(settings);
        public RepositoryPatternInMemoryBuilder<TNext, string> AddRepositoryPatternInMemoryStorageWithStringKey<TNext>(Action<RepositoryPatternBehaviorSettings<TNext, string>>? settings = default)
            => _services!.AddRepositoryPatternInMemoryStorageWithStringKey(settings);
        public RepositoryPatternInMemoryBuilder<TNext, Guid> AddRepositoryPatternInMemoryStorageWithGuidKey<TNext>(Action<RepositoryPatternBehaviorSettings<TNext, Guid>>? settings = default)
            => _services!.AddRepositoryPatternInMemoryStorageWithGuidKey(settings);
        public RepositoryPatternInMemoryBuilder<TNext, long> AddRepositoryPatternInMemoryStorageWithLongKey<TNext>(Action<RepositoryPatternBehaviorSettings<TNext, long>>? settings = default)
            => _services!.AddRepositoryPatternInMemoryStorageWithLongKey(settings);
        public RepositoryPatternInMemoryBuilder<TNext, int> AddRepositoryPatternInMemoryStorageWithIntKey<TNext>(Action<RepositoryPatternBehaviorSettings<TNext, int>>? settings = default)
            => _services!.AddRepositoryPatternInMemoryStorageWithIntKey(settings);
        public RepositoryPatternInMemoryCreatorBuilder<T, TKey> PopulateWithRandomData(Expression<Func<T, TKey>> navigationKey, int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
        {
            _services.AddSingleton<IPopulationService, PopulationService>();
            _services.AddSingleton<IPopulationServiceFactory, PopulationServiceFactory>();
            _services.AddSingleton<IInstanceCreator, InstanceCreator>();
            _services.AddSingleton<IRegexService, RegexService>();
            _services.AddSingleton<IAbstractPopulationService, AbstractPopulationService>();
            _services.AddSingleton<IArrayPopulationService, ArrayPopulationService>();
            _services.AddSingleton<IBoolPopulationService, BoolPopulationService>();
            _services.AddSingleton<IBytePopulationService, BytePopulationService>();
            _services.AddSingleton<ICharPopulationService, CharPopulationService>();
            _services.AddSingleton<IClassPopulationService, ObjectPopulationService>();
            _services.AddSingleton<IDelegatedPopulationService, DelegatedPopulationService>();
            _services.AddSingleton<IDictionaryPopulationService, DictionaryPopulationService>();
            _services.AddSingleton<IEnumerablePopulationService, EnumerablePopulationService>();
            _services.AddSingleton<IGuidPopulationService, GuidPopulationService>();
            _services.AddSingleton<IConcretizationPopulationService, ConcretizationPopulationService>();
            _services.AddSingleton<INumberPopulationService, NumberPopulationService>();
            _services.AddSingleton<IRangePopulationService, RangePopulationService>();
            _services.AddSingleton<IRegexPopulationService, RegexPopulationService>();
            _services.AddSingleton<IStringPopulationService, StringPopulationService>();
            _services.AddSingleton<ITimePopulationService, TimePopulationService>();
            ServiceProviderExtensions.AllPopulationServiceSettings.Add(new PopulationServiceSettings
            {
                PopulationServiceType = typeof(IPopulationService),
                EntityType = typeof(T),
                NumberOfElements = numberOfElements,
                BehaviorSettings = _internalBehaviorSettings,
                NumberOfElementsWhenEnumerableIsFound = numberOfElementsWhenEnumerableIsFound,
                AddElementToMemory = (key, entity) => InMemoryStorage<T, TKey>.Values.Add((TKey)key, (T)entity),
                KeyName = navigationKey.ToString().Split('.').Last()
            });
            return new(this, _internalBehaviorSettings);
        }
        public IServiceCollection Finalize()
            => _services!;
    }
}