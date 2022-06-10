using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public class RepositoryBuilder<T> : RepositoryBuilder<T, string, bool>
    {
        public RepositoryBuilder(IServiceCollection services, PatternType type) : base(services, type)
        {
        }
    }
}
