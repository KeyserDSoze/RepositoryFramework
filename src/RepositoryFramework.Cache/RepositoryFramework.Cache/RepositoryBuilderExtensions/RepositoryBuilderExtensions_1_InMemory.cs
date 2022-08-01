using RepositoryFramework;
using RepositoryFramework.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add in memory cache mechanism for your Repository or Query (CQRS), 
        /// injected directly in the IRepository<<typeparamref name="T"/>> interface
        /// or IQuery<<typeparamref name="T"/>> interface
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="settings">Settings for your cache.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithInMemoryCache<T>(
           this IRepositoryBuilder<T> builder,
           Action<CacheOptions<T, string>>? settings = null)
        {
            builder.Services.AddMemoryCache();
            return builder.WithCache<T, InMemoryCache<T>>(settings, ServiceLifetime.Singleton);
        }
    }
}
