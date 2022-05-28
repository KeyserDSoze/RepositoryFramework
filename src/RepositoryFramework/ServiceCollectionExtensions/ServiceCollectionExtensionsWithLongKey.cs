using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternWithLongKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, ILongableRepository<T>
               => services
                    .AddServiceWithLifeTime<ILongableRepository<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddCommandPatternWithLongKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, ILongableCommand<T>
                => services
                    .AddServiceWithLifeTime<ILongableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryPatternWithLongKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, ILongableQuery<T>
                => services
                    .AddServiceWithLifeTime<ILongableQuery<T>, TStorage>(serviceLifetime);
        public static RepositoryPatternInMemoryBuilder<T, long> AddRepositoryPatternInMemoryStorageWithLongKey<T>(
            this IServiceCollection services,
            Action<RepositoryPatternBehaviorSettings<T, long>>? settings = default)
        => services.AddRepositoryPatternInMemoryStorage(settings);
    }
}