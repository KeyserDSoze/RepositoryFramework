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
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/> client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// The key of your model is of object type.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepositoryClientSpecificInterceptor<T, TInterceptor>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T, object>
            => serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped<IRepositoryClientInterceptor<T, object>, TInterceptor>(),
                ServiceLifetime.Transient => services.AddTransient<IRepositoryClientInterceptor<T, object>, TInterceptor>(),
                ServiceLifetime.Singleton => services.AddSingleton<IRepositoryClientInterceptor<T, object>, TInterceptor>(),
                _ => services.AddScoped<IRepositoryClientInterceptor<T, object>, TInterceptor>()
            };
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/>, string client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
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
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/>, Guid client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
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
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/>, int client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
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
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/>, long client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
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