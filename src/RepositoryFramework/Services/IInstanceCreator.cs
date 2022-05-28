using RepositoryFramework.Population;

namespace RepositoryFramework.Services
{
    public interface IInstanceCreator
    {
        object? CreateInstance(Type type, IPopulationService populationService, int numberOfEntities, string treeName);
    }
}