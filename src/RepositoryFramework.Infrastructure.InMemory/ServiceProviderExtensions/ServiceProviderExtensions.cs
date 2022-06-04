using RepositoryFramework.InMemory.Population;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// It populates every object in memory storage injected. You have to use it after service collection build in your startup.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <returns>IServiceProvider</returns>
        public static IServiceProvider Populate(this IServiceProvider serviceProvider)
        {
            foreach (var service in ServiceInstall.PopulationStrategyRetriever)
            {
                var populationStrategy = service.Invoke(serviceProvider);
                if (populationStrategy != null)
                    populationStrategy.Populate();
            }
            return serviceProvider;
        }
    }
}