namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    internal sealed class DataverseIntegrations
    {
        public List<IDataverseOptions> Options { get; set; } = new();
        public static DataverseIntegrations Instance { get; } = new();
        private DataverseIntegrations() { }
    }
}
