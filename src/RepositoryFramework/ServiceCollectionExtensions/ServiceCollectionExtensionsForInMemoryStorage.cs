using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static RepositoryPatternInMemoryBuilder<T, TKey> AddRepositoryPatternInMemoryStorage<T, TKey>(
            this IServiceCollection services,
            Action<RepositoryPatternBehaviorSettings<T, TKey>>? settings = default)
            where TKey : notnull
        {
            var options = new RepositoryPatternBehaviorSettings<T, TKey>();
            settings?.Invoke(options);
            Check(options.ExceptionOddsForQuery);
            Check(options.ExceptionOddsForInsert);
            Check(options.ExceptionOddsForUpdate);
            Check(options.ExceptionOddsForGet);
            Check(options.ExceptionOddsForDelete);
            services.AddSingleton(options);
            if (typeof(TKey) == typeof(string))
            {
                services.AddRepositoryPatternWithStringKey<T, InMemoryStringableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandPatternWithStringKey<T, InMemoryStringableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryPatternWithStringKey<T, InMemoryStringableStorage<T>>(ServiceLifetime.Singleton);
            }
            else if (typeof(TKey) == typeof(int))
            {
                services.AddRepositoryPatternWithIntKey<T, InMemoryIntableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandPatternWithIntKey<T, InMemoryIntableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryPatternWithIntKey<T, InMemoryIntableStorage<T>>(ServiceLifetime.Singleton);
            }
            else if (typeof(TKey) == typeof(long))
            {
                services.AddRepositoryPatternWithLongKey<T, InMemoryLongableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandPatternWithLongKey<T, InMemoryLongableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryPatternWithLongKey<T, InMemoryLongableStorage<T>>(ServiceLifetime.Singleton);
            }
            else if (typeof(TKey) == typeof(Guid))
            {
                services.AddRepositoryPatternWithGuidKey<T, InMemoryGuidableStorage<T>>(ServiceLifetime.Singleton);
                services.AddCommandPatternWithGuidKey<T, InMemoryGuidableStorage<T>>(ServiceLifetime.Singleton);
                services.AddQueryPatternWithGuidKey<T, InMemoryGuidableStorage<T>>(ServiceLifetime.Singleton);
            }
            else
            {
                services.AddRepositoryPattern<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
                services.AddCommandPattern<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
                services.AddQueryPattern<T, TKey, InMemoryStorage<T, TKey>>(ServiceLifetime.Singleton);
            }

            return new RepositoryPatternInMemoryBuilder<T, TKey>(services);

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