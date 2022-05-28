namespace RepositoryFramework.Population
{
    public interface IRandomPopulationService
    {
        dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args);
    }
}