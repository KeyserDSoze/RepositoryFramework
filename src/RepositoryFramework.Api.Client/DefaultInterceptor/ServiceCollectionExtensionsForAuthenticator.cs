using RepositoryFramework;
using RepositoryFramework.Api.Client;
using RepositoryFramework.Api.Client.DefaultInterceptor;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add global JWT interceptor for all repository clients. Interceptor runs before every request.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddApiClientAuthorizationInterceptor(this IServiceCollection services,
            Action<AuthenticatorSettings>? settings = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var options = new AuthenticatorSettings();
            settings?.Invoke(options);
            services.AddSingleton(options);
            return services.AddService<IRepositoryClientInterceptor, Authenticator>(serviceLifetime);
        }
        /// <summary>
        /// Add JWT specific interceptor for your <typeparamref name="T"/> client. Interceptor runs before every request.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="settings">Settings.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> AddApiClientSpecificAuthorizationInterceptor<T>(
            this RepositoryBuilder<T> builder,
            Action<AuthenticatorSettings<T>>? settings = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var options = new AuthenticatorSettings<T>();
            settings?.Invoke(options);
            builder
               .Services
               .AddSingleton(options)
               .AddService<IRepositoryClientInterceptor<T>, Authenticator<T>>(serviceLifetime);
            return builder;
        }
        /// <summary>
        /// Add JWT specific interceptor for your <typeparamref name="T"/> client. Interceptor runs before every request.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="settings">Settings.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T, TKey> AddApiClientSpecificAuthorizationInterceptor<T, TKey>(
            this RepositoryBuilder<T, TKey> builder,
            Action<AuthenticatorSettings<T>>? settings = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TKey : notnull
        {
            var options = new AuthenticatorSettings<T>();
            settings?.Invoke(options);
            builder
                .Services
                .AddService<IRepositoryClientInterceptor<T>, Authenticator<T>>(serviceLifetime);
            return builder;
        }
        /// <summary>
        /// Add JWT specific interceptor for your <typeparamref name="T"/> client. Interceptor runs before every request.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param>
        /// <param name="settings">Settings.</param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static RepositoryBuilder<T, TKey, TState> AddApiClientSpecificAuthorizationInterceptor<T, TKey, TState>(
            this RepositoryBuilder<T, TKey, TState> builder,
            Action<AuthenticatorSettings<T>>? settings = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TKey : notnull
        {
            var options = new AuthenticatorSettings<T>();
            settings?.Invoke(options);
            builder
              .Services
              .AddService<IRepositoryClientInterceptor<T>, Authenticator<T>>(serviceLifetime);
            return builder;
        }
    }
}