namespace RepositoryFramework.InMemory.Population
{
    public interface IPopulationStrategy<T, TKey> : IPopulationStrategy
        where TKey : notnull
    {
    }
}
