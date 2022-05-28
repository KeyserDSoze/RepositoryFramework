using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IGuidableRepositoryPattern<T>
            => services
                .AddServiceWithLifeTime<IGuidableRepositoryPattern<T>, TStorage>(serviceLifetime);

        public static IServiceCollection AddCommandPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IGuidableCommandPattern<T>
                => services
                    .AddServiceWithLifeTime<IGuidableCommandPattern<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IGuidableQueryPattern<T>
                => services
                    .AddServiceWithLifeTime<IGuidableQueryPattern<T>, TStorage>(serviceLifetime);
        public static RepositoryPatternInMemoryBuilder<T, Guid> AddRepositoryPatternInMemoryStorageWithGuidKey<T>(
            this IServiceCollection services,
            Action<RepositoryPatternBehaviorSettings<T, Guid>>? settings = default)
                => services.AddRepositoryPatternInMemoryStorage(settings);
    }
}