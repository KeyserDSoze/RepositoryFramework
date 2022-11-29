using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Web.Components
{
    public interface IRepositoryUiBuilder
    {
        IServiceCollection Services { get; }
        IRepositoryUiBuilder WithDefault<T>();
    }
}
