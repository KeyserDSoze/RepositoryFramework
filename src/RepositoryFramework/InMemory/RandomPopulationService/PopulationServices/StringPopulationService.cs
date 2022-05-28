namespace RepositoryFramework.Population
{
    internal class StringPopulationService<T, TKey> : IStringPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
            => $"{treeName}_{Guid.NewGuid()}";
    }
}