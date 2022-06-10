using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder with bool as default TState.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public class RepositoryBuilder<T, TKey> : RepositoryBuilder<T, TKey, bool>
        where TKey : notnull
    {
        public RepositoryBuilder(IServiceCollection services, PatternType type) : base(services, type)
        {
        }
    }
}
