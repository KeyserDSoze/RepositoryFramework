﻿using RepositoryFramework;
using RepositoryFramework.Api.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add specific interceptor for your <typeparamref name="T"/> client. Interceptor runs before every request.
        /// For example you can add here your JWT retrieve for authorized requests.
        /// </summary>
        /// <typeparam name="TInterceptor">Interceptor service.</typeparam>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="serviceLifetime">Service Lifetime.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> AddApiClientSpecificInterceptor<T, TKey, TInterceptor>(this IRepositoryBuilder<T, TKey> builder,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TInterceptor : class, IRepositoryClientInterceptor<T>
            where TKey : notnull
        {
            builder
                .Services
                .AddService<IRepositoryClientInterceptor<T>, TInterceptor>(serviceLifetime);
            return builder;
        }
    }
}
