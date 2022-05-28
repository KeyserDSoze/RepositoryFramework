namespace RepositoryFramework.Population
{
    internal class GuidPopulationService<T, TKey> : IGuidPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
            => Guid.NewGuid();
    }
}