using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryWithLongKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, ILongableRepository<T>
               => services
                    .AddServiceWithLifeTime<ILongableRepository<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddCommandWithLongKey<T, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, ILongableCommand<T>
                => services
                    .AddServiceWithLifeTime<ILongableCommand<T>, TStorage>(serviceLifetime);
        public static IServiceCollection AddQueryWithLongKey<T, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, ILongableQuery<T>
                => services
                    .AddServiceWithLifeTime<ILongableQuery<T>, TStorage>(serviceLifetime);
    }
}