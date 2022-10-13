namespace RepositoryFramework.Infrastructure.MsSql
{
    internal sealed class MsSqlIntegrations
    {
        public List<IMsSqlOptions> Options { get; set; } = new();
        public static MsSqlIntegrations Instance { get; } = new();
        private MsSqlIntegrations() { }
    }
}
