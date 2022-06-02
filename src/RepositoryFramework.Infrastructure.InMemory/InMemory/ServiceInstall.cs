namespace RepositoryFramework.Population
{
    internal class ServiceInstall
    {
        public static List<Func<IServiceProvider, IPopulationStrategy?>> PopulationStrategyRetriever { get; } = new();
    }
}
