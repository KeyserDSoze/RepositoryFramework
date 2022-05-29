namespace RepositoryFramework.Population
{
    public interface IRandomPopulationService
    {
        int Priority { get; }
        bool IsValid(Type type);
        dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args);
    }
}