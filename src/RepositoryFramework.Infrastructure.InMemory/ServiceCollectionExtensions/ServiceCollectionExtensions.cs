using RepositoryFramework;
using RepositoryFramework.InMemory;
using RepositoryFramework.InMemory.Population;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add in memory integration (for test purpose) with bool as state.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="settings">
        /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
        /// You may set a list of exceptions with a random percentage of throwing.
        /// </param>
        /// <returns>RepositoryInMemoryBuilder</returns>
        public static IRepositoryInMemoryBuilder<T, TKey> AddRepositoryInMemoryStorage<T, TKey>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, TKey>>? settings = default)
            where TKey : notnull
        {
            var options = new RepositoryBehaviorSettings<T, TKey>();
            settings?.Invoke(options);
            CheckSettings(options);
            services.AddSingleton(options);
            services.AddRepository<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            services.AddCommand<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            services.AddQuery<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            services.AddEventAfterServiceCollectionBuild(serviceProvider =>
            {
                var populationStrategy = serviceProvider.GetService<IPopulationStrategy<T, TKey>>();
                if (populationStrategy != null)
                    populationStrategy.Populate();
                return Task.CompletedTask;
            });
            return new RepositoryInMemoryBuilder<T, TKey>(services);
        }
    }
}
