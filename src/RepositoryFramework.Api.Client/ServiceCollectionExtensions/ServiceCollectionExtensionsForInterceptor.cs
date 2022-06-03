using RepositoryFramework.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add global interceptor for all repository clients. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
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
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/>, <typeparamref name="TKey"/> client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
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
    }
}