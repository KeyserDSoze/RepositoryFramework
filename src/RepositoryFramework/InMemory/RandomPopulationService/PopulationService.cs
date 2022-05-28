namespace RepositoryFramework.Population
{
    internal class PopulationService : IPopulationService
    {
        private readonly IPopulationServiceFactory _factory;
        private readonly IDelegatedPopulationService _delegatedPopulationService;
        private readonly IConcretizationPopulationService _implementationPopulationService;
        private readonly IRegexPopulationService _regexPopulationService;

        public PopulationService(IPopulationServiceFactory factory,
            IDelegatedPopulationService delegatedPopulationService,
            IRegexPopulationService regexPopulationService,
            IConcretizationPopulationService implementationPopulationService)
        {
            _factory = factory;
            _delegatedPopulationService = delegatedPopulationService;
            _implementationPopulationService = implementationPopulationService;
            _regexPopulationService = regexPopulationService;
        }
        public dynamic? Construct(Type type, int numberOfEntities, string treeName, string propertyName, InternalBehaviorSettings settings)
        {
            treeName = string.IsNullOrWhiteSpace(treeName) ? propertyName :
                (string.IsNullOrWhiteSpace(propertyName) ? treeName : $"{treeName}.{propertyName}");

            int? overridedNumberOfEntities = null;
            var numberOfEntitiesDictionary = settings.NumberOfElements;
            if (numberOfEntitiesDictionary.ContainsKey(treeName))
                overridedNumberOfEntities = numberOfEntitiesDictionary[treeName];
            numberOfEntities = overridedNumberOfEntities ?? numberOfEntities;

            if (settings.DelegatedMethodForValueCreation.ContainsKey(treeName))
                return _delegatedPopulationService.GetValue(type, this, numberOfEntities, treeName,
                    settings,
                    settings.DelegatedMethodForValueCreation[treeName]);

            if (settings.RegexForValueCreation.ContainsKey(treeName))
                return _regexPopulationService.GetValue(type, this, numberOfEntities, treeName,
                    settings,
                    settings.RegexForValueCreation[treeName]);

            if (settings.AutoIncrementations.ContainsKey(treeName))
                return settings.AutoIncrementations[treeName]++;

            if (settings.ImplementationForValueCreation.ContainsKey(treeName) && !string.IsNullOrWhiteSpace(propertyName))
                return _implementationPopulationService.GetValue(type, this, numberOfEntities, treeName,
                    settings,
                    settings.ImplementationForValueCreation[treeName]);

            var service = _factory.GetService(type, treeName);
            if (service != default)
                return service.GetValue(type, this, numberOfEntities, treeName, settings, null!);
            return default;
        }
    }
}