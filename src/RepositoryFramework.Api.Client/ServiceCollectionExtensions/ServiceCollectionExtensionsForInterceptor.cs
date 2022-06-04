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
            => services.AddService<IRepositoryClientInterceptor, TInterceptor>(serviceLifetime);
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/> client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryClientSpecificInterceptor<T, TInterceptor>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T>
            => services.AddService<IRepositoryClientInterceptor<T>, TInterceptor>(serviceLifetime);
    }
}