namespace RepositoryFramework.Population
{
    public interface IDictionaryPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}