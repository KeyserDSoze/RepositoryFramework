using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add in memory and distributed cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="inMemorySettings">Settings for your cache.</param>
        /// <param name="distributedSettings">Settings for your cache.</param>
        /// <param name="inMemoryLifetime">Service Lifetime.</param>
        /// <param name="distributedLifetime">Service Lifetime.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithInMemoryAndDistributedCache<T, TKey>(
           this IRepositoryBuilder<T, TKey> builder,
           Action<CacheOptions<T, TKey, State<T>>>? inMemorySettings = null,
           Action<CacheOptions<T, TKey, State<T>>>? distributedSettings = null,
           ServiceLifetime inMemoryLifetime = ServiceLifetime.Singleton,
           ServiceLifetime distributedLifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            => builder
                .WithCache<T, TKey, InMemoryCache<T, TKey>>(inMemorySettings, inMemoryLifetime)
                .WithDistributedCache(distributedSettings, distributedLifetime);
    }
}
