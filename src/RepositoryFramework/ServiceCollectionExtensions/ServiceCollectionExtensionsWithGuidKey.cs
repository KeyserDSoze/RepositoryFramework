using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IGuidableRepository<T>
            => services
                .AddServiceWithLifeTime<IGuidableRepository<T>, TStorage>(serviceLifetime);

        public static IServiceCollection AddCommandPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IGuidableCommand<T>
                => services
                    .AddServiceWithLifeTime<IGuidableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IGuidableQuery<T>
                => services
                    .AddServiceWithLifeTime<IGuidableQuery<T>, TStorage>(serviceLifetime);
        public static RepositoryPatternInMemoryBuilder<T, Guid> AddRepositoryPatternInMemoryStorageWithGuidKey<T>(
            this IServiceCollection services,
            Action<RepositoryPatternBehaviorSettings<T, Guid>>? settings = default)
                => services.AddRepositoryPatternInMemoryStorage(settings);
    }
}