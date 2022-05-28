namespace RepositoryFramework.Population
{
    public interface IPopulationService
    {
        dynamic? Construct(Type type, int numberOfEntities, string treeName, string name);
    }
    public interface IPopulationService<T, TKey> : IPopulationService
    {
    }
}