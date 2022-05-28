namespace RepositoryFramework.Population
{
    internal class DelegatedPopulationService : IDelegatedPopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => args.Invoke();
    }
}