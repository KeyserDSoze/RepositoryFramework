using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    /// <summary>
    /// Population service to allow the in memory population.
    /// </summary>
    public interface IPopulationService
    {
        BehaviorSettings Settings { get; set; }
        IInstanceCreator InstanceCreator { get; }
        dynamic? Construct(Type type, int numberOfEntities, string treeName, string name);
    }
}