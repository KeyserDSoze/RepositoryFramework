using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder with State as default TState.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    internal class RepositoryBuilder<T, TKey> : RepositoryBuilder<T, TKey, State<T>>, IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        public RepositoryBuilder(IServiceCollection services, PatternType type, ServiceLifetime serviceLifetime)
            : base(services, type, serviceLifetime)
        {
        }
    }
}
