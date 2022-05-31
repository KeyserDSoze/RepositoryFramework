using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryWithGuidKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IGuidableRepository<T>
            => services
                .AddServiceWithLifeTime<IGuidableRepository<T>, TStorage>(serviceLifetime);

        public static IServiceCollection AddCommandWithGuidKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IGuidableCommand<T>
                => services
                    .AddServiceWithLifeTime<IGuidableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryWithGuidKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IGuidableQuery<T>
                => services
                    .AddServiceWithLifeTime<IGuidableQuery<T>, TStorage>(serviceLifetime);
    }
}