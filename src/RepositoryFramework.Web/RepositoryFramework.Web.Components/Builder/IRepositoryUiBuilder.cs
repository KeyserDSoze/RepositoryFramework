using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Web.Components
{
    public interface IRepositoryUiBuilder
    {
        IServiceCollection Services { get; }
        IRepositoryUiBuilder WithDefault<T>();
        IRepositoryUiConfigurationBuilder<T, TKey> Configure<T, TKey>() where TKey : notnull;
    }
}
