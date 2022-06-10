using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TCache">Implementation of your cache.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T, TKey> WithCache<T, TKey, TCache>(
           this RepositoryBuilder<T, TKey> builder,
           Action<CacheOptions<T, TKey, bool>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TCache : class, ICache<T, TKey>
        {
            var options = new CacheOptions<T, TKey, bool>();
            settings?.Invoke(options);
            builder.Services
                .AddService<ICache<T, TKey>, TCache>(lifetime)
                .AddSingleton(options);
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .AddService<IRepository<T, TKey>, CachedRepository<T, TKey>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .AddService<IQuery<T, TKey>, CachedQuery<T, TKey>>(builder.ServiceLifetime);
            return builder;
        }
        /// <summary>
        /// Add distributed (for multi-instance environments) cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TCache">Implementation of your cache.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T, TKey> WithDistributedCache<T, TKey, TCache>(
           this RepositoryBuilder<T, TKey> builder,
           Action<DistributedCacheOptions<T, TKey, bool>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TCache : class, IDistributedCache<T, TKey>
        {
            var options = new DistributedCacheOptions<T, TKey, bool>();
            settings?.Invoke(options);
            builder.Services
                .AddService<IDistributedCache<T, TKey>, TCache>(lifetime)
                .AddSingleton(options);
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .AddService<IRepository<T, TKey>, CachedRepository<T, TKey>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .AddService<IQuery<T, TKey>, CachedQuery<T, TKey>>(builder.ServiceLifetime);
            return builder;
        }
    }
}
