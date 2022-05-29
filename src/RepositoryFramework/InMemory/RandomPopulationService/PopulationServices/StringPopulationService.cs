namespace RepositoryFramework.Population
{
    internal class StringPopulationService : IRandomPopulationService
    {
        public int Priority => 4;

        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => $"{treeName}_{Guid.NewGuid()}";

        public bool IsValid(Type type) 
            => type == typeof(string);
    }
}