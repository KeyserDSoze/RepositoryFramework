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
            return this;
        }

        public IRepositoryUiConfigurationBuilder<T, TKey> Configure<T, TKey>()
            where TKey : notnull
            => new RepositoryUiConfigurationBuilder<T, TKey>(this);
    }
}
