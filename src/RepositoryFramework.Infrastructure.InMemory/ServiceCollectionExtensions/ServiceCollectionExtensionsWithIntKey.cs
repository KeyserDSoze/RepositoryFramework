using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add in memory integration (for test purpose).
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="settings">
        /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
        /// You may set a list of exceptions with a random percentage of throwing.
        /// </param>
        /// <returns>RepositoryInMemoryBuilder</returns>
        public static RepositoryInMemoryBuilder<T, int> AddRepositoryInMemoryStorageWithIntKey<T>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, int>>? settings = default)
        => services.AddRepositoryInMemoryStorage(settings);
    }
}