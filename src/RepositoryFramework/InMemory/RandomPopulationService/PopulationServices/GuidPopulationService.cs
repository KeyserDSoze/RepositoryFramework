namespace RepositoryFramework.Population
{
    internal class GuidPopulationService : IRandomPopulationService
    {
        public int Priority => 1;

        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => Guid.NewGuid();

        public bool IsValid(Type type) 
            => type == typeof(Guid) || type == typeof(Guid?);
    }
}