namespace RepositoryFramework.Population
{
    internal class StringPopulationService : IStringPopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => $"{treeName}_{Guid.NewGuid()}";
    }
}