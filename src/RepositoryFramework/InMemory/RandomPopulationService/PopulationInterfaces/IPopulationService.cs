using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    public interface IPopulationService
    {
        IInstanceCreator InstanceCreator { get; }
        dynamic? Construct(Type type, int numberOfEntities, string treeName, string name, InternalBehaviorSettings settings);
    }
}