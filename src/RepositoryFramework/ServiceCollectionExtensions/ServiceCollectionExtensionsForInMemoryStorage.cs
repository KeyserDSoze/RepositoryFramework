using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static RepositoryInMemoryBuilder<T, TKey> AddRepositoryInMemoryStorage<T, TKey>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, TKey>>? settings = default)
            where TKey : notnull 
            => services.AddRepositoryInMemoryStorage(false, settings);
        private static RepositoryInMemoryBuilder<T, TKey> AddRepositoryInMemoryStorage<T, TKey>(
            this IServiceCollection services,
            bool isSpecific,
            Action<RepositoryBehaviorSettings<T, TKey>>? settings)
            where TKey : notnull
        {
            var options = new RepositoryBehaviorSettings<T, TKey>();
            settings?.Invoke(options);
            Check(options.ExceptionOddsForQuery);
            Check(options.ExceptionOddsForInsert);
            Check(options.ExceptionOddsForUpdate);
            Check(options.ExceptionOddsForGet);
            Check(options.ExceptionOddsForDelete);
            services.AddSingleton(options);
            Type keyType = typeof(TKey);
            if (isSpecific && keyType == typeof(string))
            {
                services.AddRepositoryWithStringKey<T, InMemoryStringableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandWithStringKey<T, InMemoryStringableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryWithStringKey<T, InMemoryStringableStorage<T>>(ServiceLifetime.Singleton);
            }
            else if (isSpecific && keyType == typeof(int))
            {
                services.AddRepositoryWithIntKey<T, InMemoryIntableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandWithIntKey<T, InMemoryIntableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryWithIntKey<T, InMemoryIntableStorage<T>>(ServiceLifetime.Singleton);
            }
            else if (isSpecific && keyType == typeof(long))
            {
                services.AddRepositoryWithLongKey<T, InMemoryLongableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandWithLongKey<T, InMemoryLongableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryWithLongKey<T, InMemoryLongableStorage<T>>(ServiceLifetime.Singleton);
            }
            else if (isSpecific && keyType == typeof(Guid))
            {
                services.AddRepositoryWithGuidKey<T, InMemoryGuidableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandWithGuidKey<T, InMemoryGuidableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryWithGuidKey<T, InMemoryGuidableStorage<T>>(ServiceLifetime.Singleton);
            }
            else
            {
                services.AddRepository<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
                services.AddCommand<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
                services.AddQuery<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            }

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