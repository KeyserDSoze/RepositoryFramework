using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add IDistributedCache you installed in your DI for your Repository or Query (CQRS) cache mechanism, 
        /// based on what you injected in DI during startup, IDistributedCache interface
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <param name="lifetime">Service Lifetime.</param>
        /// <returns>RepositoryBuilder<<typeparamref name="T"/>></returns>
        public static RepositoryBuilder<T> WithDistributedCache<T>(
           this RepositoryBuilder<T> builder,
           Action<CacheOptions<T, string, State>>? settings = null,
           ServiceLifetime lifetime = ServiceLifetime.Singleton)
            => builder.WithDistributedCache<T, DistributedCache<T>>(settings, lifetime);
    }
}
