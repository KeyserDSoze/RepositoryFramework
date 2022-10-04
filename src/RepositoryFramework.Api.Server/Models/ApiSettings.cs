using System.Reflection;

namespace RepositoryFramework
{
    internal sealed class ApiSettings
    {
        public static ApiSettings Instance { get; } = new ApiSettings();
        private ApiSettings() { }
        public string Name { get; set; } = Assembly.GetExecutingAssembly().GetName().Name!;
        public string Path { get; set; } = "api";
        public string? Version { get; set; }
        public string StartingPath => $"{Path}{(string.IsNullOrWhiteSpace(Version) ? string.Empty : $"/{Version}")}";
        public bool HasDocumentation { get; set; }
        public bool HasSwagger { get; set; }
        public ApiIdentitySettings OpenIdIdentity { get; set; } = new();
        public bool HasOpenIdAuthentication => OpenIdIdentity.HasOpenIdAuthentication;
    }
}
