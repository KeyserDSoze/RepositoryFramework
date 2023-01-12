using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public interface IRepositorySettings<T, TKey>
        where TKey : notnull
    {
        IServiceCollection Services { get; }
        PatternType Type { get; }
        /// <summary>
        /// It's a parameter used by framework to understand the level of privacy,
        /// for instance, it's used in library Api.Server to avoid auto creation of an api with this repository implementation.
        /// </summary>
        void SetNotExposable();
        IRepositoryBuilder<T, TKey, TStorage> SetStorage<TStorage>(PatternType type, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IRepository<T, TKey>;
        IRepositoryBuilder<T, TKey, TStorage> SetRepositoryStorage<TStorage>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IRepository<T, TKey>;
        IRepositoryBuilder<T, TKey, TStorage> SetCommandStorage<TStorage>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, ICommand<T, TKey>;
        IRepositoryBuilder<T, TKey, TStorage> SetQueryStorage<TStorage>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IQuery<T, TKey>;
    }
}
