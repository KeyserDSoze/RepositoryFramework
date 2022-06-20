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
            for (int i = 0; i < _settings.NumberOfElements; i++)
            {
                var entity = _instanceCreator!.CreateInstance(new RandomPopulationOptions(typeof(T),
                    _populationService!, _settings.NumberOfElementsWhenEnumerableIsFound, string.Empty));
                foreach (var property in properties.Where(x => x.CanWrite))
                    if (property.PropertyType == typeof(Range) ||
                            GetDefault(property.PropertyType) == (property.GetValue(entity) as dynamic))
                        property.SetValue(entity, _populationService!.Construct(property.PropertyType,
                            _settings.NumberOfElementsWhenEnumerableIsFound, string.Empty,
                            property.Name));

                var key = properties.First(x => x.Name == _settings.KeyName).GetValue(entity);
                _settings.AddElementToMemory?.Invoke((TKey)key!, (T)entity!);
            }
        }
        private static dynamic GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                var t = Activator.CreateInstance(type);
                return t;
            }
            return null;
        }
    }
}
