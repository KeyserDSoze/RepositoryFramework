namespace RepositoryFramework.Population
{
    public interface IPopulationServiceFactory
    {
        IRandomPopulationService GetService(Type type, string treeName);
    }
}