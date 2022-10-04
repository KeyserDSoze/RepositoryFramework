using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public interface IApiBuilder
    {
        IServiceCollection Services { get; }
        IApiBuilder WithName(string name);
        IApiBuilder WithPath(string path);
        IApiBuilder WithVersion(string version);
        IApiBuilder WithDocumentation();
        IApiBuilder WithSwagger();
        IPolicyApiBuilder WithOpenIdAuthentication(Action<ApiIdentitySettings> settings);
    }
}
