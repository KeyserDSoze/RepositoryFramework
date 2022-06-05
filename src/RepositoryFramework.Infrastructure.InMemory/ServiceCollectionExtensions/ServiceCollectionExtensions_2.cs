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
        public static RepositoryInMemoryBuilder<T, TKey, bool> AddRepositoryInMemoryStorage<T, TKey>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, TKey, bool>>? settings = default)
            where TKey : notnull
        {
            services.AddRepository<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            services.AddCommand<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            services.AddQuery<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);

            return services.AddRepositoryInMemoryStorage(PopulationOfState, settings);
        }
        private static bool PopulationOfState(bool result, Exception exception)
        {
            if (result)
                return true;
            else if (exception != null)
                throw exception;
            return false;
        }
    }
}