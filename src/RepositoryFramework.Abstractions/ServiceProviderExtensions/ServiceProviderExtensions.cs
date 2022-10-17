using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Call it after the build of your IServiceCollection if you added some work with AddEventAfterServiceCollectionBuild during DI.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <returns>IServiceProvider</returns>
        public static async Task<IServiceProvider> AfterBuildAsync(this IServiceProvider serviceProvider, bool alltogether = false)
        {
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;
            List<Task> tasks = new();
            foreach (var service in RepositoryFrameworkAfterServiceBuildEvents.Instance.Events)
            {
                if (alltogether)
                    tasks.Add(service.Invoke(serviceProvider));
                else
                    await service.Invoke(serviceProvider);
            }
            await Task.WhenAll(tasks).NoContext();
            return serviceProvider;
        }
    }
}
