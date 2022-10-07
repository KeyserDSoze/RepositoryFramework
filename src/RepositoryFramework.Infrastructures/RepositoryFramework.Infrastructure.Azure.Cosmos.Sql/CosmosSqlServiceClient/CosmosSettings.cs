using Microsoft.Azure.Cosmos;

namespace RepositoryFramework.Infrastructure.Azure.Cosmos.Sql
{
    /// <summary>
    /// Settings for your cosmos db and container.
    /// </summary>
    public sealed class CosmosSettings
    {
        public Uri? EndpointUri { get; set; }
        public string? IdentityClientId { get; set; }
        public string? ConnectionString { get; set; }
        public string DatabaseName { get; set; } = null!;
        public string? ContainerName { get; set; }
        public CosmosClientOptions? ClientOptions { get; set; }
        public CosmosOptions? DatabaseOptions { get; set; }
        public CosmosOptions? ContainerOptions { get; set; }
    }
}
