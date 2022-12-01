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
            AppInternalSettings.Instance.RootName = typeof(T).Name;
            return this;
        }
        public IRepositoryUiBuilder WithAuthentication()
        {
            AppInternalSettings.Instance.IsAuthenticated = true;
            return this;
        }

        public IRepositoryUiConfigurationBuilder<T, TKey> Configure<T, TKey>()
            where TKey : notnull
            => new RepositoryUiConfigurationBuilder<T, TKey>(this);

        public IRepositoryUiBuilder DoNotExpose<T>()
        {
            AppInternalSettings.Instance.NotExposableRepositories.Add(typeof(T).Name);
            return this;
        }
    }
}
