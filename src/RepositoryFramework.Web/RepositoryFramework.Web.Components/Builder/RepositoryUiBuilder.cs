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
            AppConstant.Instance.RootName = typeof(T).Name;
            Services.AddRazorPages(x =>
            {
                x.Conventions.AddPageRoute($"/Repository/{typeof(T).Name}/Query", "/");
            });
            return this;
        }
    }
}
