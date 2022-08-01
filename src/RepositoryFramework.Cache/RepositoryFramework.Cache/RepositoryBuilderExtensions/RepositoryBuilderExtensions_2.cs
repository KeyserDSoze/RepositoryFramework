using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        private static void AddCacheManager<T, TKey>(this IRepositoryBuilder<T, TKey> builder,
            CacheOptions<T, TKey> options)
            where TKey : notnull
        {
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .RemoveServiceIfAlreadyInstalled<CachedRepository<T, TKey>>(typeof(IRepository<T, TKey>))
                    .AddService<IRepository<T, TKey>, CachedRepository<T, TKey>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .RemoveServiceIfAlreadyInstalled<CachedQuery<T, TKey>>(typeof(IQuery<T, TKey>))
                    .AddService<IQuery<T, TKey>, CachedQuery<T, TKey>>(builder.ServiceLifetime);
            else if (options.HasCommandPattern)
                builder.Services
                    .RemoveServiceIfAlreadyInstalled<CachedRepository<T, TKey>>(typeof(ICommand<T, TKey>))
                    .AddService<ICommand<T, TKey>, CachedRepository<T, TKey>>(builder.ServiceLifetime);
        }
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
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithCache<T, TKey, TCache>(
           this IRepositoryBuilder<T, TKey> builder,
           Action<CacheOptions<T, TKey>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TCache : class, ICache<T, TKey>
        {
            var options = new CacheOptions<T, TKey>();
            settings?.Invoke(options);
            builder.Services
                .RemoveServiceIfAlreadyInstalled<TCache>(typeof(ICache<T, TKey>))
                .AddService<ICache<T, TKey>, TCache>(lifetime)
                .AddSingleton(options);
            builder.AddCacheManager(options);
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
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithDistributedCache<T, TKey, TCache>(
           this IRepositoryBuilder<T, TKey> builder,
           Action<DistributedCacheOptions<T, TKey>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TCache : class, IDistributedCache<T, TKey>
        {
            var options = new DistributedCacheOptions<T, TKey>();
            settings?.Invoke(options);
            builder.Services
                .RemoveServiceIfAlreadyInstalled<TCache>(typeof(IDistributedCache<T, TKey>))
                .AddService<IDistributedCache<T, TKey>, TCache>(lifetime)
                .AddSingleton(options);
            builder.AddCacheManager(options);
            return builder;
        }
    }
}
