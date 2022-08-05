using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.InMemory.Population;
using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryBuilder<T, TKey> : IRepositoryInMemoryBuilder<T, TKey>
        where TKey : notnull
    {
        private protected readonly CreationSettings _internalBehaviorSettings = new();
        private protected int _numberOfParameters;
        public IServiceCollection Services { get; }
        public PatternType Type => PatternType.Repository;
        public ServiceLifetime ServiceLifetime => ServiceLifetime.Singleton;
        public RepositoryInMemoryBuilder(IServiceCollection services)
        {
            Services = services;
            _numberOfParameters = 2;
        }
        private void AddElementBasedOnGenericElements(TKey key, T value)
        {
            if (_numberOfParameters == 1)
                InMemoryStorage<T>.AddValue(key.ToString()!, value);
            else
                InMemoryStorage<T, TKey>.AddValue(key, value);
        }
        public IRepositoryInMemoryBuilder<TNext, TNextKey> AddRepositoryInMemoryStorage<TNext, TNextKey>(Action<RepositoryBehaviorSettings<TNext, TNextKey>>? settings = default)
            where TNextKey : notnull
            => Services!.AddRepositoryInMemoryStorage(settings);
        public IRepositoryInMemoryBuilder<TNext> AddRepositoryInMemoryStorage<TNext>(Action<RepositoryBehaviorSettings<TNext>>? settings = default)
            => Services!.AddRepositoryInMemoryStorage(settings);
        public IRepositoryInMemoryBuilder<T, TKey> PopulateWithJsonData(
            Expression<Func<T, TKey>> navigationKey,
            string json)
        {
            var elements = json.FromJson<IEnumerable<T>>();
            if (elements != null)
                return PopulateWithDataInjection(navigationKey, elements);
            return this;
        }
        public IRepositoryInMemoryBuilder<T, TKey> PopulateWithDataInjection(
            Expression<Func<T, TKey>> navigationKey,
            IEnumerable<T> elements)
        {
            var keyProperty = navigationKey.GetPropertyBasedOnKey();
            foreach (var element in elements)
                AddElementBasedOnGenericElements((TKey)keyProperty.GetValue(element)!, element);
            return this;
        }
        public IRepositoryInMemoryCreatorBuilder<T, TKey> PopulateWithRandomData(
            Expression<Func<T, TKey>> navigationKey,
            int numberOfElements = 100,
            int numberOfElementsWhenEnumerableIsFound = 10)
        {
            Services.AddSingleton<IPopulationService, PopulationService>();
            Services.AddSingleton<IInstanceCreator, InstanceCreator>();
            Services.AddSingleton<IRegexService, RegexService>();
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
            Services.AddSingleton(PopulationServiceSelector.Instance);
            Services.AddSingleton<IPopulationStrategy<T, TKey>, RandomPopulationStrategy<T, TKey>>();
            _ = Services.AddSingleton(
                new PopulationServiceSettings<T, TKey>
                {
                    NumberOfElements = numberOfElements,
                    BehaviorSettings = _internalBehaviorSettings,
                    NumberOfElementsWhenEnumerableIsFound = numberOfElementsWhenEnumerableIsFound,
                    AddElementToMemory = AddElementBasedOnGenericElements,
                    KeyCalculator = navigationKey.Compile()
                });
            return new RepositoryInMemoryCreatorBuilder<T, TKey>(this, _internalBehaviorSettings);
        }
        public IQueryTranslationInMemoryBuilder<T, TKey, TTranslated> Translate<TTranslated>()
          => new QueryTranslationInMemoryBuilder<T, TKey, TTranslated>(this);

        IQueryTranslationBuilder<T, TKey, TTranslated> IRepositoryBuilder<T, TKey>.Translate<TTranslated>() 
            => Translate<TTranslated>();
    }
}