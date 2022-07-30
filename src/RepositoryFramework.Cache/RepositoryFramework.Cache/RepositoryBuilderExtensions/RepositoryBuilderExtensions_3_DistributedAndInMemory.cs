using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add in memory and distributed cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param>
        /// <param name="inMemorySettings">Settings for your cache.</param>
        /// <param name="distributedSettings">Settings for your cache.</param>
        /// <param name="inMemoryLifetime">Service Lifetime.</param>
        /// <param name="distributedLifetime">Service Lifetime.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static IRepositoryBuilder<T, TKey, TState> WithInMemoryAndDistributedCache<T, TKey, TState>(
           this IRepositoryBuilder<T, TKey, TState> builder,
           Action<CacheOptions<T, TKey, TState>>? inMemorySettings = null,
           Action<CacheOptions<T, TKey, TState>>? distributedSettings = null,
           ServiceLifetime inMemoryLifetime = ServiceLifetime.Singleton,
           ServiceLifetime distributedLifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TState : class, IState<T>, new()
            => builder.
                WithCache<T, TKey, TState, InMemoryCache<T, TKey, TState>>(inMemorySettings, inMemoryLifetime)
                .WithDistributedCache(distributedSettings, distributedLifetime);
    }
}
