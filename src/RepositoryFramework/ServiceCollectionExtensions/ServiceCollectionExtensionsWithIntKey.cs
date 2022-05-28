using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternWithIntKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IIntableRepositoryPattern<T>
               => services
                    .AddServiceWithLifeTime<IIntableCommandPattern<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddCommandPatternWithIntKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IIntableCommandPattern<T>
                => services
                    .AddServiceWithLifeTime<IIntableCommandPattern<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryPatternWithIntKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IIntableQueryPattern<T>
                => services
                    .AddServiceWithLifeTime<IIntableQueryPattern<T>, TStorage>(serviceLifetime);
        public static RepositoryPatternInMemoryBuilder<T, int> AddRepositoryPatternInMemoryStorageWithIntKey<T>(
            this IServiceCollection services,
            Action<RepositoryPatternBehaviorSettings<T, int>>? settings = default)
        => services.AddRepositoryPatternInMemoryStorage(settings);
    }
}