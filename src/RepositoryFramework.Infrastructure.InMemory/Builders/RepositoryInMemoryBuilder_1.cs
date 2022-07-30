using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryBuilder<T> : RepositoryInMemoryBuilder<T, string>, IRepositoryInMemoryBuilder<T>
    {
        public RepositoryInMemoryBuilder(IServiceCollection services) :
            base(services)
        {
            _numberOfParameters = 1;
        }
    }
}