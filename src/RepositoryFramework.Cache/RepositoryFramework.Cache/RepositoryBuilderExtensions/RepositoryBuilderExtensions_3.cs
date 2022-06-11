using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <typeparam name="TCache">Implementation of your cache.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static RepositoryBuilder<T, TKey, TState> WithCache<T, TKey, TState, TCache>(
           this RepositoryBuilder<T, TKey, TState> builder,
           Action<CacheOptions<T, TKey, TState>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TCache : class, ICache<T, TKey, TState>
        {
            var options = new CacheOptions<T, TKey, TState>();
            settings?.Invoke(options);
            builder.Services
                .AddService<ICache<T, TKey, TState>, TCache>(lifetime)
                .AddSingleton(options);
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .AddService<IRepository<T, TKey, TState>, CachedRepository<T, TKey, TState>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .AddService<IQuery<T, TKey, TState>, CachedQuery<T, TKey, TState>>(builder.ServiceLifetime);
            else if (options.HasCommandPattern)
                builder.Services
                    .AddService<ICommand<T, TKey, TState>, CachedRepository<T, TKey, TState>>(builder.ServiceLifetime);
            return builder;
        }
        /// <summary>
        /// Add distributed (for multi-instance environments) cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <typeparam name="TCache">Implementation of your cache.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static RepositoryBuilder<T, TKey, TState> WithDistributedCache<T, TKey, TState, TCache>(
           this RepositoryBuilder<T, TKey, TState> builder,
           Action<DistributedCacheOptions<T, TKey, TState>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TCache : class, IDistributedCache<T, TKey, TState>
        {
            var options = new DistributedCacheOptions<T, TKey, TState>();
            settings?.Invoke(options);
            builder.Services
                .AddService<IDistributedCache<T, TKey, TState>, TCache>(lifetime)
                .AddSingleton(options);
            if (builder.Type == PatternType.Repository)
                builder.Services
                    .AddService<IRepository<T, TKey, TState>, CachedRepository<T, TKey, TState>>(builder.ServiceLifetime);
            else if (builder.Type == PatternType.Query)
                builder.Services
                    .AddService<IQuery<T, TKey, TState>, CachedQuery<T, TKey, TState>>(builder.ServiceLifetime);
            else if (options.HasCommandPattern)
                builder.Services
                    .AddService<ICommand<T, TKey, TState>, CachedRepository<T, TKey, TState>>(builder.ServiceLifetime);
            return builder;
        }
    }
}
