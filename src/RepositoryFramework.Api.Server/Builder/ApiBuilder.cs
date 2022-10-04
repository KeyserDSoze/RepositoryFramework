using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    internal sealed class ApiBuilder : IApiBuilder
    {
        public IServiceCollection Services { get; }
        public ApiBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IPolicyApiBuilder WithOpenIdAuthentication(Action<ApiIdentitySettings> settings)
        {
            var options = new ApiIdentitySettings();
            settings.Invoke(options);
            ApiSettings.Instance.OpenIdIdentity = options;
            return new PolicyApiBuilder(this);
        }

        public IApiBuilder WithDocumentation()
        {
            ApiSettings.Instance.HasDocumentation = true;
            return this;
        }

        public IApiBuilder WithName(string name)
        {
            ApiSettings.Instance.Name = name;
            return this;
        }

        public IApiBuilder WithPath(string path)
        {
            ApiSettings.Instance.Path = path;
            return this;
        }

        public IApiBuilder WithSwagger()
        {
            ApiSettings.Instance.HasSwagger = true;
            _ = Services.AddSwaggerConfigurations(ApiSettings.Instance);
            return this;
        }

        public IApiBuilder WithVersion(string version)
        {
            ApiSettings.Instance.Version = version;
            return this;
        }
    }
}
