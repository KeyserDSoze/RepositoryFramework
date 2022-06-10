using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public class RepositoryBuilder<T, TKey> : RepositoryBuilder<T, TKey, bool>
        where TKey : notnull
    {
        public RepositoryBuilder(IServiceCollection services, PatternType type) : base(services, type)
        {
        }
    }
}
