using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryWithStringKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IStringableRepository<T>
               => services
                    .AddServiceWithLifeTime<IStringableRepository<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddCommandWithStringKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IStringableCommand<T>
                => services
                    .AddServiceWithLifeTime<IStringableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryWithStringKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IStringableQuery<T>
                => services
                    .AddServiceWithLifeTime<IStringableQuery<T>, TStorage>(serviceLifetime);
        public static RepositoryInMemoryBuilder<T, string> AddRepositoryInMemoryStorageWithStringKey<T>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, string>>? settings = default)
        => services.AddRepositoryInMemoryStorage(true, settings);
    }
}