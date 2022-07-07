namespace RepositoryFramework.InMemory.Population
{
    internal class PopulationService : IPopulationService
    {
        private readonly PopulationServiceSelector _selector;
        private readonly IRegexService _regexService;
        public IInstanceCreator InstanceCreator { get; }
        public CreationSettings Settings { get; set; } = null!;
        public PopulationService(PopulationServiceSelector selector,
            IRegexService regexService,
            IInstanceCreator instanceCreator)
        {
            _selector = selector;
            _regexService = regexService;
            InstanceCreator = instanceCreator;
        }

        public dynamic? Construct(Type type, int numberOfEntities, string treeName, string name)
        {
            if (!string.IsNullOrWhiteSpace(treeName) && !string.IsNullOrWhiteSpace(name))
                treeName = $"{treeName}.{name}";
            else if(!string.IsNullOrWhiteSpace(name))
                treeName = name;

            int? overridedNumberOfEntities = null;
            var numberOfEntitiesDictionary = Settings.NumberOfElements;
            if (numberOfEntitiesDictionary.ContainsKey(treeName))
                overridedNumberOfEntities = numberOfEntitiesDictionary[treeName];
            numberOfEntities = overridedNumberOfEntities ?? numberOfEntities;

            if (Settings.DelegatedMethodForValueCreation.ContainsKey(treeName))
                return Settings.DelegatedMethodForValueCreation[treeName].Invoke();

            if (Settings.RegexForValueCreation.ContainsKey(treeName))
                return _regexService.GetRandomValue(type,
                    Settings.RegexForValueCreation[treeName]);

            if (Settings.AutoIncrementations.ContainsKey(treeName))
                return Settings.AutoIncrementations[treeName]++;

            if (Settings.ImplementationForValueCreation.ContainsKey(treeName) && !string.IsNullOrWhiteSpace(name))
                return Construct(Settings.ImplementationForValueCreation[treeName], numberOfEntities,
                    treeName, string.Empty);

            var service = _selector.GetRightService(type);
            if (service != default)
                return service.GetValue(new RandomPopulationOptions(type, this, numberOfEntities, treeName));
            return default;
        }
    }
}