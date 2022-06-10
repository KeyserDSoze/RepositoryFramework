﻿using RepositoryFramework;
using RepositoryFramework.Api.Client;

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
        public static IServiceCollection AddApiClientInterceptor<TInterceptor>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor
            => services.AddService<IRepositoryClientInterceptor, TInterceptor>(serviceLifetime);
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/> client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service.</typeparam>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <param name="builder">/// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param></param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static RepositoryBuilder<T, TKey, TState> AddApiClientSpecificInterceptor<T, TKey, TState, TInterceptor>(this RepositoryBuilder<T, TKey, TState> builder,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T>
            where TKey : notnull
        {
            builder
              .ToServiceCollection()
              .AddService<IRepositoryClientInterceptor<T>, TInterceptor>(serviceLifetime);
            return builder;
        }

        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/> client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service</typeparam>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T, TKey> AddApiClientSpecificInterceptor<T, TKey, TInterceptor>(this RepositoryBuilder<T, TKey> builder,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T>
            where TKey : notnull
        {
            builder
                .ToServiceCollection()
                .AddService<IRepositoryClientInterceptor<T>, TInterceptor>(serviceLifetime);
            return builder;
        }

        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/> client. Interceptor works before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service.</typeparam>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param></param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> AddApiClientSpecificInterceptor<T, TInterceptor>(this RepositoryBuilder<T> builder,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T>
        {
            builder
               .ToServiceCollection()
               .AddService<IRepositoryClientInterceptor<T>, TInterceptor>(serviceLifetime);
            return builder;
        }
    }
}