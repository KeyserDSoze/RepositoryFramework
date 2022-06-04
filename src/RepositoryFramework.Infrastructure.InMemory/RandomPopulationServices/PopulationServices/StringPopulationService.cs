namespace RepositoryFramework.InMemory.Population
{
    internal class StringPopulationService : IRandomPopulationService
    {
        public int Priority => 4;

        public dynamic GetValue(RandomPopulationOptions options)
            => $"{options.TreeName}_{Guid.NewGuid()}";

        public bool IsValid(Type type) 
            => type == typeof(string);
    }
}