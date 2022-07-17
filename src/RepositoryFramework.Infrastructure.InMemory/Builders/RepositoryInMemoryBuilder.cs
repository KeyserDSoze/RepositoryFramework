using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.InMemory.Population;
using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.InMemory
{
    public class RepositoryInMemoryBuilder<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState
    {
        private readonly IServiceCollection _services;
        private readonly CreationSettings _internalBehaviorSettings = new();
        private readonly int _numberOfParameters;
        internal RepositoryInMemoryBuilder(IServiceCollection services, int numberOfParameters)
        {
            _services = services;
            _numberOfParameters = numberOfParameters;
        }

        public RepositoryInMemoryBuilder<TNext, TNextKey, TNextState> AddRepositoryInMemoryStorage<TNext, TNextKey, TNextState>(
            Action<RepositoryBehaviorSettings<TNext, TNextKey, TNextState>>? settings = default)
            where TNextKey : notnull
            where TNextState : class, IState
            => _services!.AddRepositoryInMemoryStorage(settings);
        public RepositoryInMemoryBuilder<TNext, TNextKey, State> AddRepositoryInMemoryStorage<TNext, TNextKey>(Action<RepositoryBehaviorSettings<TNext, TNextKey, State>>? settings = default)
            where TNextKey : notnull
            => _services!.AddRepositoryInMemoryStorage(settings);
        public RepositoryInMemoryBuilder<TNext, string, State> AddRepositoryInMemoryStorage<TNext>(Action<RepositoryBehaviorSettings<TNext, string, State>>? settings = default)
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
                        if (_numberOfParameters == 1)
                            InMemoryStorage<T>.Values.Add(key.ToString()!, entity);
                        else if (_numberOfParameters == 2)
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