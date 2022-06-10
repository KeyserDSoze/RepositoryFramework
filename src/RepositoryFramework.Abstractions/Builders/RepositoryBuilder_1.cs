using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder with string as default TKey and bool as default TState.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public class RepositoryBuilder<T> : RepositoryBuilder<T, string>
    {
        public RepositoryBuilder(IServiceCollection services, PatternType type) : base(services, type)
        {
        }
    }
}
