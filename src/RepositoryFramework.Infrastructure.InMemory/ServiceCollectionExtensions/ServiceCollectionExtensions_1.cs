using RepositoryFramework;
using RepositoryFramework.InMemory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add in memory integration (for test purpose) with string as key and bool as state.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="settings">
        /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
        /// You may set a list of exceptions with a random percentage of throwing.
        /// </param>
        /// <returns>RepositoryInMemoryBuilder</returns>
        public static RepositoryInMemoryBuilder<T, string, State<T>> AddRepositoryInMemoryStorage<T>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, string, State<T>>>? settings = default)
        {
            services.AddRepository<T, InMemoryStorage<T>>(ServiceLifetime.Singleton);
            services.AddCommand<T, InMemoryStorage<T>>(ServiceLifetime.Singleton);
            services.AddQuery<T, InMemoryStorage<T>>(ServiceLifetime.Singleton);

            return services.AddRepositoryInMemoryStorage<T, string>(options =>
            {
                settings?.Invoke(options);
                options.NumberOfParameters = 1;
            });
        }
    }
}