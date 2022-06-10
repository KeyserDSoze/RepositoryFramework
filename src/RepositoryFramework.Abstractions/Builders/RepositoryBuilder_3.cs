using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
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
