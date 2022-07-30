using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryBuilder<T, TKey> : RepositoryInMemoryBuilder<T, TKey, State<T>>, IRepositoryInMemoryBuilder<T, TKey>
        where TKey : notnull
    {
        public RepositoryInMemoryBuilder(IServiceCollection services) : base(services)
        {
            _numberOfParameters = 2;
        }
    }
}