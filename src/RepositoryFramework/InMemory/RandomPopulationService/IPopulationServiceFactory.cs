namespace RepositoryFramework.Population
{
    public interface IPopulationServiceFactory<T, TKey>
        where TKey : notnull
    {
        IRandomPopulationService<T, TKey> GetService(Type type, string treeName);
    }
}