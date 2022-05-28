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
        private readonly InternalBehaviorSettings<T, TKey> _internalBehaviorSettings = new();
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
            _services.AddSingleton<IInstanceCreator, InstanceCreator>();
            _services.AddSingleton<IPopulationService<T, TKey>, PopulationService<T, TKey>>();
            _services.AddSingleton<IPopulationServiceFactory<T, TKey>, PopulationServiceFactory<T, TKey>>();
            _services.AddSingleton<IRegexService, RegexService>();
            _services.AddSingleton<IAbstractPopulationService<T, TKey>, AbstractPopulationService<T, TKey>>();
            _services.AddSingleton<IArrayPopulationService<T, TKey>, ArrayPopulationService<T, TKey>>();
            _services.AddSingleton<IBoolPopulationService<T, TKey>, BoolPopulationService<T, TKey>>();
            _services.AddSingleton<IBytePopulationService<T, TKey>, BytePopulationService<T, TKey>>();
            _services.AddSingleton<ICharPopulationService<T, TKey>, CharPopulationService<T, TKey>>();
            _services.AddSingleton<IClassPopulationService<T, TKey>, ObjectPopulationService<T, TKey>>();
            _services.AddSingleton<IDelegatedPopulationService<T, TKey>, DelegatedPopulationService<T, TKey>>();
            _services.AddSingleton<IDictionaryPopulationService<T, TKey>, DictionaryPopulationService<T, TKey>>();
            _services.AddSingleton<IEnumerablePopulationService<T, TKey>, EnumerablePopulationService<T, TKey>>();
            _services.AddSingleton<IGuidPopulationService<T, TKey>, GuidPopulationService<T, TKey>>();
            _services.AddSingleton<IImplementationPopulationService<T, TKey>, ImplementationPopulationService<T, TKey>>();
            _services.AddSingleton<INumberPopulationService<T, TKey>, NumberPopulationService<T, TKey>>();
            _services.AddSingleton<IRangePopulationService<T, TKey>, RangePopulationService<T, TKey>>();
            _services.AddSingleton<IRegexPopulationService<T, TKey>, RegexPopulationService<T, TKey>>();
            _services.AddSingleton<IStringPopulationService<T, TKey>, StringPopulationService<T, TKey>>();
            _services.AddSingleton<ITimePopulationService<T, TKey>, TimePopulationService<T, TKey>>();
            ServiceProviderExtensions.AllPopulationServiceSettings.Add(new PopulationServiceSettings
            {
                PopulationServiceType = typeof(IPopulationService<T, TKey>),
                EntityType = typeof(T),
                NumberOfElements = numberOfElements,
                InternalSettingsType = typeof(InternalBehaviorSettings<T, TKey>),
                NumberOfElementsWhenEnumerableIsFound = numberOfElementsWhenEnumerableIsFound,
                AddElementToMemory = (key, entity) => InMemoryStorage<T, TKey>.Values.Add((TKey)key, (T)entity),
                KeyName = navigationKey.ToString().Split('.').Last()
            });
            _services.AddSingleton(_internalBehaviorSettings);
            return new(this, _internalBehaviorSettings);
        }
        public IServiceCollection Finalize()
            => _services!;
    }
}