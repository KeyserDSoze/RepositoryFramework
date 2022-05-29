namespace RepositoryFramework.Population
{
    internal class AbstractPopulationService : IRandomPopulationService
    {
        public int Priority => 0;

        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
        {
            return null!;
        }

        public bool IsValid(Type type) 
            => type.IsAbstract;
    }
}