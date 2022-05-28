namespace RepositoryFramework.Population
{
    public interface IEnumerablePopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}