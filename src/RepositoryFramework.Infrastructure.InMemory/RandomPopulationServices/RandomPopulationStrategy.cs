using System.Collections;

namespace RepositoryFramework.InMemory.Population
{
    internal class RandomPopulationStrategy<T, TKey> : IPopulationStrategy<T, TKey>
        where TKey : notnull
    {
        private readonly IPopulationService _populationService;
        private readonly IInstanceCreator _instanceCreator;
        private readonly PopulationServiceSettings<T, TKey> _settings;

        public RandomPopulationStrategy(IPopulationService populationService,
            IInstanceCreator instanceCreator,
            PopulationServiceSettings<T, TKey> settings)
        {
            _populationService = populationService;
            _instanceCreator = instanceCreator;
            _settings = settings;
        }
        public void Populate()
        {
            _populationService.Settings = _settings.BehaviorSettings ?? new();
            var properties = typeof(T).GetProperties();
            for (var i = 0; i < _settings.NumberOfElements; i++)
            {
                var entity = _instanceCreator!.CreateInstance(new RandomPopulationOptions(typeof(T),
                    _populationService!, _settings.NumberOfElementsWhenEnumerableIsFound, string.Empty));
                foreach (var property in properties.Where(x => x.CanWrite))
                {
                    if (property.PropertyType == typeof(Range) ||
                            GetDefault(property.PropertyType) == (property.GetValue(entity) as dynamic))
                    {
                        var value = _populationService!.Construct(property.PropertyType,
                            _settings.NumberOfElementsWhenEnumerableIsFound, string.Empty,
                            property.Name);
                        if (property.PropertyType.GetInterface(nameof(IList)) != null && value is IEnumerable enumerable)
                        {
                            var list = (Activator.CreateInstance(property.PropertyType) as IList)!;
                            foreach (var singleItem in enumerable)
                                list.Add(singleItem);
                            value = list;
                        }
                        property.SetValue(entity, value);
                    }
                }
                var item = (T)entity!;
                var key = _settings.KeyCalculator!.Invoke(item);
                _settings.AddElementToMemory?.Invoke(key, item);
            }
        }
        private static dynamic GetDefault(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type)!;
            return null!;
        }
    }
}
