using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Population;
using RepositoryFramework.Services;
using System.Linq.Expressions;

namespace RepositoryFramework
{
    public class RepositoryInMemoryBuilder<T, TKey>
        where TKey : notnull
    {
        private readonly IServiceCollection _services;
        private readonly BehaviorSettings _internalBehaviorSettings = new();
        public RepositoryInMemoryBuilder(IServiceCollection services)
            => _services = services;
        public RepositoryInMemoryBuilder<TNext, TNextKey> AddRepositoryInMemoryStorage<TNext, TNextKey>(Action<RepositoryBehaviorSettings<TNext, TNextKey>>? settings = default)
            where TNextKey : notnull
            => _services!.AddRepositoryInMemoryStorage(settings);
        public RepositoryInMemoryBuilder<TNext, string> AddRepositoryInMemoryStorageWithStringKey<TNext>(Action<RepositoryBehaviorSettings<TNext, string>>? settings = default)
            => _services!.AddRepositoryInMemoryStorageWithStringKey(settings);
        public RepositoryInMemoryBuilder<TNext, Guid> AddRepositoryInMemoryStorageWithGuidKey<TNext>(Action<RepositoryBehaviorSettings<TNext, Guid>>? settings = default)
            => _services!.AddRepositoryInMemoryStorageWithGuidKey(settings);
        public RepositoryInMemoryBuilder<TNext, long> AddRepositoryInMemoryStorageWithLongKey<TNext>(Action<RepositoryBehaviorSettings<TNext, long>>? settings = default)
            => _services!.AddRepositoryInMemoryStorageWithLongKey(settings);
        public RepositoryInMemoryBuilder<TNext, int> AddRepositoryInMemoryStorageWithIntKey<TNext>(Action<RepositoryBehaviorSettings<TNext, int>>? settings = default)
            => _services!.AddRepositoryInMemoryStorageWithIntKey(settings);
        public RepositoryInMemoryCreatorBuilder<T, TKey> PopulateWithRandomData(Expression<Func<T, TKey>> navigationKey, int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
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