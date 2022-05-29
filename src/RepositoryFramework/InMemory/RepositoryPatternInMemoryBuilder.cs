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
            _services.AddSingleton<IInstanceCreator, InstanceCreator>();
            _services.AddSingleton<IRegexService, RegexService>();
            PopulationServiceSelector.Instance.TryAdd(new AbstractPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new ArrayPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new BoolPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new BytePopulationService());
            PopulationServiceSelector.Instance.TryAdd(new CharPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new ObjectPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new DictionaryPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new EnumerablePopulationService());
            PopulationServiceSelector.Instance.TryAdd(new GuidPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new NumberPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new RangePopulationService());
            PopulationServiceSelector.Instance.TryAdd(new StringPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new TimePopulationService());
            _services.AddSingleton(PopulationServiceSelector.Instance);
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