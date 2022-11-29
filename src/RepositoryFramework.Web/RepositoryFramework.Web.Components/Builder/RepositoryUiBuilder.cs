using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Web.Components
{
    internal sealed class RepositoryUiBuilder : IRepositoryUiBuilder
    {
        public IServiceCollection Services { get; }
        public RepositoryUiBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IRepositoryUiBuilder WithDefault<T>()
        {
            Services.AddRazorPages(x =>
            {
                AppSettings.Instance.Root = typeof(T).Name;
                x.Conventions.AddPageRoute($"/Repository/{typeof(T).Name}/Query", "/");
            });
            return this;
        }
    }
}
