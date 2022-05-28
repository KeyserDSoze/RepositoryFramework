namespace RepositoryFramework.Population
{
    internal class PopulationService<T, TKey> : IPopulationService<T, TKey>
        where TKey : notnull
    {
        private readonly IPopulationServiceFactory<T, TKey> _factory;
        private readonly InternalBehaviorSettings<T, TKey> _settings;
        private readonly IDelegatedPopulationService<T, TKey> _delegatedPopulationService;
        private readonly IImplementationPopulationService<T, TKey> _implementationPopulationService;
        private readonly IRegexPopulationService<T, TKey> _regexPopulationService;
        public PopulationService(IPopulationServiceFactory<T, TKey> factory,
            InternalBehaviorSettings<T, TKey> settings,
            IDelegatedPopulationService<T, TKey> delegatedPopulationService,
            IRegexPopulationService<T, TKey> regexPopulationService,
            IImplementationPopulationService<T, TKey> implementationPopulationService
            )
        {
            _factory = factory;
            _settings = settings;
            _delegatedPopulationService = delegatedPopulationService;
            _implementationPopulationService = implementationPopulationService;
            _regexPopulationService = regexPopulationService;
        }
        public dynamic? Construct(Type type, int numberOfEntities, string treeName, string propertyName)
        {
            treeName = string.IsNullOrWhiteSpace(treeName) ? propertyName :
                (string.IsNullOrWhiteSpace(propertyName) ? treeName : $"{treeName}.{propertyName}");

            int? overridedNumberOfEntities = null;
            var numberOfEntitiesDictionary = _settings.NumberOfElements;
            if (numberOfEntitiesDictionary.ContainsKey(treeName))
                overridedNumberOfEntities = numberOfEntitiesDictionary[treeName];
            numberOfEntities = overridedNumberOfEntities ?? numberOfEntities;

            if (_settings.DelegatedMethodForValueCreation.ContainsKey(treeName))
                return _delegatedPopulationService.GetValue(type, this, numberOfEntities, treeName,
                    _settings.DelegatedMethodForValueCreation[treeName]);

            if (_settings.RegexForValueCreation.ContainsKey(treeName))
                return _regexPopulationService.GetValue(type, this, numberOfEntities, treeName,
                    _settings.RegexForValueCreation[treeName]);

            if (_settings.ImplementationForValueCreation.ContainsKey(treeName) && !string.IsNullOrWhiteSpace(propertyName))
                return _implementationPopulationService.GetValue(type, this, numberOfEntities, treeName,
                    _settings.ImplementationForValueCreation[treeName]); 

            var service = _factory.GetService(type, treeName);
            if (service != default)
                return service.GetValue(type, this, numberOfEntities, treeName, null!);
            return default;
        }
    }
}