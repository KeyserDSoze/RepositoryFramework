namespace RepositoryFramework.Population
{
    public interface IImplementationPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}