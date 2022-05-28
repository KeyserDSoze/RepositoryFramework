using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternWithIntKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IIntableRepository<T>
               => services
                    .AddServiceWithLifeTime<IIntableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddCommandPatternWithIntKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IIntableCommand<T>
                => services
                    .AddServiceWithLifeTime<IIntableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryPatternWithIntKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IIntableQuery<T>
                => services
                    .AddServiceWithLifeTime<IIntableQuery<T>, TStorage>(serviceLifetime);
        public static RepositoryPatternInMemoryBuilder<T, int> AddRepositoryPatternInMemoryStorageWithIntKey<T>(
            this IServiceCollection services,
            Action<RepositoryPatternBehaviorSettings<T, int>>? settings = default)
        => services.AddRepositoryPatternInMemoryStorage(settings);
    }
}