namespace RepositoryFramework.Population
{
    public interface IArrayPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}