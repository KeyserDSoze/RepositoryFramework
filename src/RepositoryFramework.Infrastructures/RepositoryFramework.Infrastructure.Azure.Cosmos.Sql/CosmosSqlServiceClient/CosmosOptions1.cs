namespace RepositoryFramework.Infrastructure.Azure.Cosmos.Sql
{
    public sealed class CosmosOptions<T, TKey>
    {
        public string ContainerName { get; }
        public string ExistQuery { get; }
        public CosmosOptions(string containerName)
        {
            ContainerName = containerName;
            ExistQuery = $"SELECT * FROM {containerName} x WHERE x.id = @id";
        }
    }
}
