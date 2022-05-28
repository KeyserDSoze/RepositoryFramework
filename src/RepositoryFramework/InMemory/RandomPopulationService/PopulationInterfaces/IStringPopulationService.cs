namespace RepositoryFramework.Population
{
    public interface IStringPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}