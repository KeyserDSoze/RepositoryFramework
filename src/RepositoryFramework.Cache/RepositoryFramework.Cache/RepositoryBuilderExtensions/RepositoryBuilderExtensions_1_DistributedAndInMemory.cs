using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add in memory and distributed cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="inMemorySettings">Settings for your cache.</param>
        /// <param name="distributedSettings">Settings for your cache.</param>
        /// <param name="inMemoryLifetime">Service Lifetime.</param>
        /// <param name="distributedLifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> WithInMemoryAndDistributedCache<T>(
           this RepositoryBuilder<T> builder,
           Action<CacheOptions<T, string, State<T>>>? inMemorySettings = null,
           Action<CacheOptions<T, string, State<T>>>? distributedSettings = null,
           ServiceLifetime inMemoryLifetime = ServiceLifetime.Singleton,
           ServiceLifetime distributedLifetime = ServiceLifetime.Singleton)
            => builder.
                WithCache<T, InMemoryCache<T>>(inMemorySettings, inMemoryLifetime)
                .WithDistributedCache(distributedSettings, distributedLifetime);
    }
}
