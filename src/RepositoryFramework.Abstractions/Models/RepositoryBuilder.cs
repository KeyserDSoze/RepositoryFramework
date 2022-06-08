using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public sealed class RepositoryBuilder<T, TKey, TState>
        where TKey : notnull
    {
        private readonly IServiceCollection _services;
        public RepositoryBuilder(IServiceCollection services)
        {
            _services = services;
        }
        public IServiceCollection ToServiceCollection()
            => _services;
    }
}
