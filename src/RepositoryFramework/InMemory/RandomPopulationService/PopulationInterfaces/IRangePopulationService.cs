namespace RepositoryFramework.Population
{
    public interface IRangePopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}