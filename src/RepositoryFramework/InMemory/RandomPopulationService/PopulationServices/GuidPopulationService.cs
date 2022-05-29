namespace RepositoryFramework.Population
{
    internal class GuidPopulationService : IRandomPopulationService
    {
        public int Priority => 1;

        public dynamic GetValue(RandomPopulationOptions options)
            => Guid.NewGuid();

        public bool IsValid(Type type) 
            => type == typeof(Guid) || type == typeof(Guid?);
    }
}