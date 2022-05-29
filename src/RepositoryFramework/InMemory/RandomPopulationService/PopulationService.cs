using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    internal class PopulationService : IPopulationService
    {
        private readonly PopulationServiceSelector _selector;
        private readonly IRegexService _regexService;
        public IInstanceCreator InstanceCreator { get; }

        public PopulationService(PopulationServiceSelector selector,
            IRegexService regexService,
            IInstanceCreator instanceCreator)
        {
            _selector = selector;
            _regexService = regexService;
            InstanceCreator = instanceCreator;
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
                return settings.DelegatedMethodForValueCreation[treeName].Invoke();

            if (settings.RegexForValueCreation.ContainsKey(treeName))
                return _regexService.GetRandomValue(type,
                    settings.RegexForValueCreation[treeName]);

            if (settings.AutoIncrementations.ContainsKey(treeName))
                return settings.AutoIncrementations[treeName]++;

            if (settings.ImplementationForValueCreation.ContainsKey(treeName) && !string.IsNullOrWhiteSpace(propertyName))
                return Construct(settings.ImplementationForValueCreation[treeName], numberOfEntities,
                    treeName, string.Empty, settings);

            var service = _selector.GetRightService(type);
            if (service != default)
                return service.GetValue(type, this, numberOfEntities, treeName, settings, null!);
            return default;
        }
    }
}