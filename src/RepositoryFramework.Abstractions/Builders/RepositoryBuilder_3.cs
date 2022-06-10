using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public class RepositoryBuilder<T, TKey, TState>
        where TKey : notnull
    {
        private readonly IServiceCollection _services;
        public PatternType Type { get; }
        public RepositoryBuilder(IServiceCollection services, PatternType type)
        {
            _services = services;
            Type = type;
        }
        public IServiceCollection ToServiceCollection()
            => _services;
    }
}
