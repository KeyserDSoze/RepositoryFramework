using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternWithStringKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IStringableRepository<T>
               => services
                    .AddServiceWithLifeTime<IStringableRepository<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddCommandPatternWithStringKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IStringableCommand<T>
                => services
                    .AddServiceWithLifeTime<IStringableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryPatternWithStringKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IStringableQuery<T>
                => services
                    .AddServiceWithLifeTime<IStringableQuery<T>, TStorage>(serviceLifetime);
        public static RepositoryPatternInMemoryBuilder<T, string> AddRepositoryPatternInMemoryStorageWithStringKey<T>(
            this IServiceCollection services,
            Action<RepositoryPatternBehaviorSettings<T, string>>? settings = default)
        => services.AddRepositoryPatternInMemoryStorage(true, settings);
    }
}