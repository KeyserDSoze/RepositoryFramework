namespace RepositoryFramework.Population
{
    public interface IRandomPopulationService<T, TKey>
        where TKey : notnull
    {
        dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args);
    }
}