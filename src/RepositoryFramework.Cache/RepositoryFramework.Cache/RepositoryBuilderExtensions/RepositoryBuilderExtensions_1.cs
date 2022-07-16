using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
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
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> WithCache<T, TCache>(
           this RepositoryBuilder<T> builder,
           Action<CacheOptions<T, string, State>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TCache : class, ICache<T>
        {
            var options = new CacheOptions<T, string, State>();
            settings?.Invoke(options);
            builder.Services
                .AddService<ICache<T>, TCache>(lifetime)
                .AddSingleton(options);
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .AddService<IRepository<T>, CachedRepository<T>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .AddService<IQuery<T>, CachedQuery<T>>(builder.ServiceLifetime);
            else if(options.HasCommandPattern)
                builder.Services
                    .AddService<ICommand<T>, CachedRepository<T>>(builder.ServiceLifetime);
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
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static RepositoryBuilder<T> WithDistributedCache<T, TCache>(
           this RepositoryBuilder<T> builder,
           Action<DistributedCacheOptions<T, string, State>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TCache : class, IDistributedCache<T>
        {
            var options = new DistributedCacheOptions<T, string, State>();
            settings?.Invoke(options);
            builder.Services
                .AddService<IDistributedCache<T>, TCache>(lifetime)
                .AddSingleton(options);
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .AddService<IRepository<T>, CachedRepository<T>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .AddService<IQuery<T>, CachedQuery<T>>(builder.ServiceLifetime);
            else if (options.HasCommandPattern)
                builder.Services
                    .AddService<ICommand<T>, CachedRepository<T>>(builder.ServiceLifetime);
            return builder;
        }
    }
}
