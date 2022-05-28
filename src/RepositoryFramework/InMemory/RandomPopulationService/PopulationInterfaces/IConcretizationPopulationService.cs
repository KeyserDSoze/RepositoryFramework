namespace RepositoryFramework.Population
{
    public interface IConcretizationPopulationService<T, TKey> : IRandomPopulationService<T, TKey>
        where TKey : notnull
    { }
}