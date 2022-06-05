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
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="settings">
        /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
        /// You may set a list of exceptions with a random percentage of throwing.
        /// </param>
        /// <returns>RepositoryInMemoryBuilder</returns>
        public static RepositoryInMemoryBuilder<T, TKey, TState> AddRepositoryInMemoryStorage<T, TKey, TState>(
            this IServiceCollection services,
            Func<bool, Exception, TState>? populationOfState,
            Action<RepositoryBehaviorSettings<T, TKey, TState>>? settings = default)
            where TKey : notnull
        {
            InMemoryRepositoryInstalled.PopulationStrategyRetriever.Add((serviceProvider) => serviceProvider.GetService<IPopulationStrategy<T, TKey>>());
            var options = new RepositoryBehaviorSettings<T, TKey, TState>();
            settings?.Invoke(options);
            options.PopulationOfState = populationOfState;
            Check(options.Get(RepositoryMethod.Insert).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Update).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Delete).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Get).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Query).ExceptionOdds);
            Check(options.Get(RepositoryMethod.Exist).ExceptionOdds);
            Check(options.Get(RepositoryMethod.All).ExceptionOdds);
            services.AddSingleton(options);
            services.AddRepository<T, TKey, TState, InMemoryStorage<T, TKey, TState>>(ServiceLifetime.Singleton);
            services.AddCommand<T, TKey, TState, InMemoryStorage<T, TKey, TState>>(ServiceLifetime.Singleton);
            services.AddQuery<T, TKey, TState, InMemoryStorage<T, TKey, TState>>(ServiceLifetime.Singleton);

            return new RepositoryInMemoryBuilder<T, TKey, TState>(services, options.NumberOfParameters ?? 3);

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