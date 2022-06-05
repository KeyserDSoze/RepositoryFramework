namespace RepositoryFramework.InMemory.Population
{
    internal class InMemoryRepositoryInstalled
    {
        public static List<Func<IServiceProvider, IPopulationStrategy?>> PopulationStrategyRetriever { get; } = new();
    }
}
