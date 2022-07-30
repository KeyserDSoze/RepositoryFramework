using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        private static void AddCacheManager<T>(this IRepositoryBuilder<T> builder,
            CacheOptions<T, string, State<T>> options)
        {
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .RemoveServiceIfAlreadyInstalled<CachedRepository<T>>(typeof(IRepository<T>))
                    .AddService<IRepository<T>, CachedRepository<T>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .RemoveServiceIfAlreadyInstalled<CachedQuery<T>>(typeof(IQuery<T>))
                    .AddService<IQuery<T>, CachedQuery<T>>(builder.ServiceLifetime);
            else if (options.HasCommandPattern)
                builder.Services
                    .RemoveServiceIfAlreadyInstalled<CachedRepository<T>>(typeof(ICommand<T>))
                    .AddService<ICommand<T>, CachedRepository<T>>(builder.ServiceLifetime);
        }
        /// <summary>
        /// Add cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TCache">Implementation of your cache.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithCache<T, TCache>(
           this IRepositoryBuilder<T> builder,
           Action<CacheOptions<T, string, State<T>>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TCache : class, ICache<T>
        {
            var options = new CacheOptions<T, string, State<T>>();
            settings?.Invoke(options);
            builder.Services
                .RemoveServiceIfAlreadyInstalled<TCache>(typeof(ICache<T>))
                .AddService<ICache<T>, TCache>(lifetime)
                .AddSingleton(options);
            builder.AddCacheManager(options);
            return builder;
        }
        /// <summary>
        /// Add distributed (for multi-instance environments) cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>, > interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TCache">Implementation of your cache.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T> WithDistributedCache<T, TCache>(
           this IRepositoryBuilder<T> builder,
           Action<DistributedCacheOptions<T, string, State<T>>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TCache : class, IDistributedCache<T>
        {
            var options = new DistributedCacheOptions<T, string, State<T>>();
            settings?.Invoke(options);
            builder.Services
                .RemoveServiceIfAlreadyInstalled<TCache>(typeof(IDistributedCache<T>))
                .AddService<IDistributedCache<T>, TCache>(lifetime)
                .AddSingleton(options);
            builder.AddCacheManager(options);
            return builder;
        }
    }
}
