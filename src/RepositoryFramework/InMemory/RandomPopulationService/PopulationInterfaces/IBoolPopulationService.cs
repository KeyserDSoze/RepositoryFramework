namespace RepositoryFramework.Population
{
    public interface IBoolPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}