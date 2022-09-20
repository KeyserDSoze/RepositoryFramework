using Microsoft.Extensions.Configuration;

namespace RepositoryFramework
{
    public sealed class ApiIdentitySettings
    {
        public Uri? AuthorizationUrl { get; set; }
        public Uri? TokenUrl { get; set; }
        public string? ClientId { get; set; }
        public List<ApiIdentityScopeSettings> Scopes { get; set; } = new();
        public bool HasOpenIdAuthentication => AuthorizationUrl != null;
        /// <summary>
        /// Set configuration for Azure Active Directory based on classic configuration in appsettings
        /// "AzureAd": {
        /// "Instance": "https://login.microsoftonline.com/",
        /// "Domain": "your domain",
        /// "TenantId": "usually in secrets",
        /// "ClientId": "usually in secrets",
        /// "ClientSecret": "usually in secrets",
        /// "Scopes": "usually in secrets",
        /// "CallbackPath": "/signin-oidc"
        /// }
        /// </summary>
        /// <param name="configuration">IConfiguration instance.</param>
        public void ConfigureAzureActiveDirectory(IConfiguration configuration)
        {
            AuthorizationUrl = new Uri($"{configuration["AzureAd:Instance"]}{configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize");
            TokenUrl = new Uri($"{configuration["AzureAd:Instance"]}{configuration["AzureAd:TenantId"]}/oauth2/v2.0/token");
            ClientId = configuration["AzureAd:ClientId"];
            Scopes.AddRange(configuration["AzureAd:Scopes"].Split(' ')
                .Select(x => new ApiIdentityScopeSettings()
                {
                    Value = x,
                    Description = x
                }));
        }
    }
}
