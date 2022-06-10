using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public class RepositoryBuilder<T, TKey, TState>
        where TKey : notnull
    {
        public IServiceCollection Services { get; }
        public PatternType Type { get; }
        public RepositoryBuilder(IServiceCollection services, PatternType type)
        {
            Services = services;
            Type = type;
        }
    }
}
