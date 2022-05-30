using RepositoryFramework.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryClientInterceptor<TInterceptor>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor
            => serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped<IRepositoryClientInterceptor, TInterceptor>(),
                ServiceLifetime.Transient => services.AddTransient<IRepositoryClientInterceptor, TInterceptor>(),
                ServiceLifetime.Singleton => services.AddSingleton<IRepositoryClientInterceptor, TInterceptor>(),
                _ => services.AddScoped<IRepositoryClientInterceptor, TInterceptor>()
            };
        public static IServiceCollection AddRepositoryClientSpecificInterceptor<T, TKey, TInterceptor>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T, TKey>
            where TKey : notnull
            => serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped<IRepositoryClientInterceptor<T, TKey>, TInterceptor>(),
                ServiceLifetime.Transient => services.AddTransient<IRepositoryClientInterceptor<T, TKey>, TInterceptor>(),
                ServiceLifetime.Singleton => services.AddSingleton<IRepositoryClientInterceptor<T, TKey>, TInterceptor>(),
                _ => services.AddScoped<IRepositoryClientInterceptor<T, TKey>, TInterceptor>()
            };
        public static IServiceCollection AddRepositoryClientSpecificInterceptorForStringKey<T, TInterceptor>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T, string>
            => serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped<IRepositoryClientInterceptor<T, string>, TInterceptor>(),
                ServiceLifetime.Transient => services.AddTransient<IRepositoryClientInterceptor<T, string>, TInterceptor>(),
                ServiceLifetime.Singleton => services.AddSingleton<IRepositoryClientInterceptor<T, string>, TInterceptor>(),
                _ => services.AddScoped<IRepositoryClientInterceptor<T, string>, TInterceptor>()
            };
        public static IServiceCollection AddRepositoryClientSpecificInterceptorForGuidKey<T, TInterceptor>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T, Guid>
            => serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped<IRepositoryClientInterceptor<T, Guid>, TInterceptor>(),
                ServiceLifetime.Transient => services.AddTransient<IRepositoryClientInterceptor<T, Guid>, TInterceptor>(),
                ServiceLifetime.Singleton => services.AddSingleton<IRepositoryClientInterceptor<T, Guid>, TInterceptor>(),
                _ => services.AddScoped<IRepositoryClientInterceptor<T, Guid>, TInterceptor>()
            };
        public static IServiceCollection AddRepositoryClientSpecificInterceptorForIntKey<T, TInterceptor>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TInterceptor : class, IRepositoryClientInterceptor<T, int>
           => serviceLifetime switch
           {
               ServiceLifetime.Scoped => services.AddScoped<IRepositoryClientInterceptor<T, int>, TInterceptor>(),
               ServiceLifetime.Transient => services.AddTransient<IRepositoryClientInterceptor<T, int>, TInterceptor>(),
               ServiceLifetime.Singleton => services.AddSingleton<IRepositoryClientInterceptor<T, int>, TInterceptor>(),
               _ => services.AddScoped<IRepositoryClientInterceptor<T, int>, TInterceptor>()
           };
        public static IServiceCollection AddRepositoryClientSpecificInterceptorForLongKey<T, TInterceptor>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TInterceptor : class, IRepositoryClientInterceptor<T, long>
           => serviceLifetime switch
           {
               ServiceLifetime.Scoped => services.AddScoped<IRepositoryClientInterceptor<T, long>, TInterceptor>(),
               ServiceLifetime.Transient => services.AddTransient<IRepositoryClientInterceptor<T, long>, TInterceptor>(),
               ServiceLifetime.Singleton => services.AddSingleton<IRepositoryClientInterceptor<T, long>, TInterceptor>(),
               _ => services.AddScoped<IRepositoryClientInterceptor<T, long>, TInterceptor>()
           };
    }
}