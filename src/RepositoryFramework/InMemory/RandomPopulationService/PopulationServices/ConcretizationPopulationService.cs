namespace RepositoryFramework.Population
{
    internal class ConcretizationPopulationService : IConcretizationPopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => populationService.Construct(args, numberOfEntities, treeName, string.Empty, settings)!;
    }
}