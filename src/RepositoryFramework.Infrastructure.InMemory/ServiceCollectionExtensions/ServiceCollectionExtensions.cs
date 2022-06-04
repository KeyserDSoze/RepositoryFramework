using RepositoryFramework;
using RepositoryFramework.InMemory;
using RepositoryFramework.InMemory.Population;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add in memory integration (for test purpose).
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="settings">
        /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
        /// You may set a list of exceptions with a random percentage of throwing.
        /// </param>
        /// <returns>RepositoryInMemoryBuilder</returns>
        public static RepositoryInMemoryBuilder<T, TKey> AddRepositoryInMemoryStorage<T, TKey>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, TKey>>? settings = default)
            where TKey : notnull
        {
            ServiceInstall.PopulationStrategyRetriever.Add((serviceProvider) => serviceProvider.GetService<IPopulationStrategy<T, TKey>>());
            var options = new RepositoryBehaviorSettings<T, TKey>();
            settings?.Invoke(options);
            Check(options.Get(RepositoryMethod.Insert).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Update).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Delete).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Get).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Query).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Exist).ExceptionOdds);
            Check(options.Get(RepositoryMethod.All).ExceptionOdds);
            services.AddSingleton(options);
            Type keyType = typeof(TKey);
            services.AddRepository<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            services.AddCommand<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            services.AddQuery<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);

            return new RepositoryInMemoryBuilder<T, TKey>(services);

            static void Check(List<ExceptionOdds> odds)
            {
                var total = odds.Sum(x => x.Percentage);
                if (odds.Where(x => x.Percentage <= 0 || x.Percentage > 100).Any())
                {
                    throw new ArgumentException("Some percentages are wrong, greater than 100% or lesser than 0.");
                }
                if (total > 100)
                    throw new ArgumentException("Your total percentage is greater than 100.");
            }
        }
    }
}