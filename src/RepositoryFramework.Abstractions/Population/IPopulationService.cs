using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    public interface IPopulationService
    {
        BehaviorSettings Settings { get; set; }
        IInstanceCreator InstanceCreator { get; }
        dynamic? Construct(Type type, int numberOfEntities, string treeName, string name);
    }
}