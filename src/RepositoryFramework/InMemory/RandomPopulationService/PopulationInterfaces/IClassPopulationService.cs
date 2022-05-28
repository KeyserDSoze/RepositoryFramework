namespace RepositoryFramework.Population
{
    public interface IClassPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}