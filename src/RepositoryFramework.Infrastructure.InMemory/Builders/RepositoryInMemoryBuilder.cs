using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.InMemory.Population;
using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.InMemory
{
    public class RepositoryInMemoryBuilder<T, TKey, TState>
        where TKey : notnull
    {
        private readonly IServiceCollection _services;
        private readonly CreationSettings _internalBehaviorSettings = new();
        public RepositoryInMemoryBuilder(IServiceCollection services)
            => _services = services;
        public RepositoryInMemoryBuilder<TNext, TNextKey, TNextState> AddRepositoryInMemoryStorage<TNext, TNextKey, TNextState>(
            Func<bool, Exception, TNextState>? populationOfState,
            Action<RepositoryBehaviorSettings<TNext, TNextKey, TNextState>>? settings = default)
            where TNextKey : notnull
            => _services!.AddRepositoryInMemoryStorage(populationOfState, settings);
        public RepositoryInMemoryBuilder<TNext, TNextKey, bool> AddRepositoryInMemoryStorage<TNext, TNextKey>(Action<RepositoryBehaviorSettings<TNext, TNextKey, bool>>? settings = default)
            where TNextKey : notnull
            => _services!.AddRepositoryInMemoryStorage(settings);
        public RepositoryInMemoryBuilder<TNext, string, bool> AddRepositoryInMemoryStorage<TNext>(Action<RepositoryBehaviorSettings<TNext, string, bool>>? settings = default)
            => _services!.AddRepositoryInMemoryStorage(settings);
        public RepositoryInMemoryBuilder<T, TKey, TState> PopulateWithJsonData(
            Expression<Func<T, TKey>> navigationKey,
            string json)
        {
            var elements = JsonSerializer.Deserialize<IEnumerable<T>>(json);
            if (elements != null)
                return PopulateWithDataInjection(navigationKey, elements);
            return this;
        }
        public RepositoryInMemoryBuilder<T, TKey, TState> PopulateWithDataInjection(
            Expression<Func<T, TKey>> navigationKey,
            IEnumerable<T> elements)
        {
            var keyType = navigationKey.GetPropertyBasedOnKey();
            foreach (var element in elements)
                InMemoryStorage<T, TKey>.Values.Add((TKey)keyType.GetValue(element)!, element);
            return this;
        }
        public RepositoryInMemoryCreatorBuilder<T, TKey, TState> PopulateWithRandomData(
            Expression<Func<T, TKey>> navigationKey,
            int numberOfElements = 100,
            int numberOfElementsWhenEnumerableIsFound = 10)
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
            _services.AddSingleton<IPopulationStrategy<T, TKey>, RandomPopulationStrategy<T, TKey>>();
            _ = _services.AddSingleton(
                new PopulationServiceSettings<T, TKey>
                {
                    NumberOfElements = numberOfElements,
                    BehaviorSettings = _internalBehaviorSettings,
                    NumberOfElementsWhenEnumerableIsFound = numberOfElementsWhenEnumerableIsFound,
                    AddElementToMemory = (key, entity) =>
                    {
                        if (typeof(TState) == typeof(bool) && typeof(TKey) == typeof(string))
                            InMemoryStorage<T>.Values.Add(key.ToString()!, entity);
                        else if (typeof(TState) == typeof(bool))
                            InMemoryStorage<T, TKey>.Values.Add(key, entity);
                        else
                            InMemoryStorage<T, TKey, TState>.Values.Add(key, entity);
                    },
                    KeyName = navigationKey.ToString().Split('.').Last()
                });
            return new(this, _internalBehaviorSettings);
        }
        public IServiceCollection ToServiceCollection()
            => _services!;
    }
}