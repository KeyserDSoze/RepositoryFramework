using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryWithIntKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IIntableRepository<T>
               => services
                    .AddServiceWithLifeTime<IIntableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddCommandWithIntKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IIntableCommand<T>
                => services
                    .AddServiceWithLifeTime<IIntableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryWithIntKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IIntableQuery<T>
                => services
                    .AddServiceWithLifeTime<IIntableQuery<T>, TStorage>(serviceLifetime);
    }
}