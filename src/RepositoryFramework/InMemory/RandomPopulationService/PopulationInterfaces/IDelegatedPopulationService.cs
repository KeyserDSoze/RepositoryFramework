namespace RepositoryFramework.Population
{
    public interface IDelegatedPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}