using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    internal class RepositoryBuilder<T, TKey, TState> : IRepositoryBuilder<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        public IServiceCollection Services { get; }
        public PatternType Type { get; }
        public ServiceLifetime ServiceLifetime { get; }
        public RepositoryBuilder(IServiceCollection services, PatternType type, ServiceLifetime serviceLifetime)
        {
            Services = services;
            Type = type;
            ServiceLifetime = serviceLifetime;
        }
    }
}
