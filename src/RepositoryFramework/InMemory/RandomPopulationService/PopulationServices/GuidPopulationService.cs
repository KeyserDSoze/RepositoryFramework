namespace RepositoryFramework.Population
{
    internal class GuidPopulationService : IGuidPopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => Guid.NewGuid();
    }
}