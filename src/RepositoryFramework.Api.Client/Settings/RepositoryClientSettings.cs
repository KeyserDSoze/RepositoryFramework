namespace RepositoryFramework.Api.Client
{
    internal sealed class RepositoryClientSettings
    {
        public static RepositoryClientSettings Instance { get; } = new();
        public Dictionary<string, RepositorySingleClientSettings> Clients { get; } = new();
        private RepositoryClientSettings()
        {
        }
    }
}
