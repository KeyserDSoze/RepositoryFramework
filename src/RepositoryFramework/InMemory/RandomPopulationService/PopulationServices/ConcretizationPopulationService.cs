namespace RepositoryFramework.Population
{
    internal class ConcretizationPopulationService<T, TKey> : IConcretizationPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
            => populationService.Construct(args, numberOfEntities, treeName, string.Empty)!;
    }
}