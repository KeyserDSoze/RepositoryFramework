namespace RepositoryFramework.Population
{
    public interface IBytePopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}