namespace RepositoryFramework.InMemory.Population
{
    internal static class InMemoryRepositoryInstalled
    {
        public static List<Func<IServiceProvider, IPopulationStrategy?>> PopulationStrategyRetriever { get; } = new();
    }
}
