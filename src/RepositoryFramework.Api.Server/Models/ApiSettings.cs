using System.Reflection;

namespace RepositoryFramework
{
    public sealed class ApiSettings
    {
        public string Name { get; set; } = Assembly.GetExecutingAssembly().GetName().Name!;
        public string StartingPath { get; set; } = "api";
        public string Version { get; set; } = "v1";
        public bool HasDocumentation { get; set; }
        public bool HasSwagger { get; set; }
        public ApiIdentitySettings Identity { get; set; } = new();
        public bool HasOpenIdAuthentication => Identity.HasOpenIdAuthentication;
    }
}
