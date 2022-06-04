namespace RepositoryFramework.InMemory.Population
{
    public interface IPopulationStrategy<T, TKey, TState> : IPopulationStrategy
        where TKey : notnull
    {
    }
}
