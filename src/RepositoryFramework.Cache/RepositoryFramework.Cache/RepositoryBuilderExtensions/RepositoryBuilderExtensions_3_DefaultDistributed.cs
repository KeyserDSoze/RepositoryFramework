using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add IDistributedCache you installed in your DI for your Repository or Query (CQRS) cache mechanism, 
        /// injected directly in the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// or IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TState">Returning state.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="TState"/>></returns>
        public static RepositoryBuilder<T, TKey, TState> WithDistributedCache<T, TKey, TState>(
           this RepositoryBuilder<T, TKey, TState> builder,
           Action<CacheOptions<T, TKey, TState>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TKey : notnull
            where TState : IState
            => builder.WithDistributedCache<T, TKey, TState, DistributedCache<T, TKey, TState>>(settings, lifetime);
    }
}
