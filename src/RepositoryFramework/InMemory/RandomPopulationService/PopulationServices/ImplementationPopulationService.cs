namespace RepositoryFramework.Population
{
    internal class ImplementationPopulationService<T, TKey> : IImplementationPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
            => populationService.Construct(args, numberOfEntities, treeName, string.Empty)!;
    }
}