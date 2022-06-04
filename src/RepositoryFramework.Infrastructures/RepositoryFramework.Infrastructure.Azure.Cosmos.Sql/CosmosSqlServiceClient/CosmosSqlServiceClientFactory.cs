using Azure.Identity;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Reflection;

namespace RepositoryFramework.Infrastructure.Azure.Cosmos.Sql
{
    public class CosmosSqlServiceClientFactory
    {
        public static CosmosSqlServiceClientFactory Instance { get; } = new CosmosSqlServiceClientFactory();
        private CosmosSqlServiceClientFactory() { }
        private readonly Dictionary<string, (Container Container, PropertyInfo[] Properties)> _containerServices = new();
        public (Container Container, PropertyInfo[] Properties) Get(string name)
            => _containerServices[name];
        internal CosmosSqlServiceClientFactory Add<T>(string databaseName, string name, string keyName, Uri endpoint, CosmosClientOptions? clientOptions, CosmosOptions? databaseOptions, CosmosOptions? containerOptions)
        {
            CosmosClient cosmosClient = new(endpoint.AbsoluteUri, new DefaultAzureCredential(), clientOptions);
            return Add<T>(databaseName, name, keyName, cosmosClient, databaseOptions, containerOptions);
        }
        internal CosmosSqlServiceClientFactory Add<T>(string databaseName, string name, string keyName, string connectionString, CosmosClientOptions? clientOptions, CosmosOptions? databaseOptions, CosmosOptions? containerOptions)
        {
            CosmosClient cosmosClient = new(connectionString, clientOptions);
            return Add<T>(databaseName, name, keyName, cosmosClient, databaseOptions, containerOptions);
        }
        private CosmosSqlServiceClientFactory Add<T>(string databaseName, string name, string keyName, CosmosClient cosmosClient, CosmosOptions? databaseOptions, CosmosOptions? containerOptions)
        {
            if (!_containerServices.ContainsKey(name))
            {
                var databaseResponse = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName,
                    databaseOptions?.ThroughputProperties,
                    databaseOptions?.RequestOptions)
                        .ConfigureAwait(false).GetAwaiter().GetResult();
                if (databaseResponse.StatusCode == HttpStatusCode.OK || databaseResponse.StatusCode == HttpStatusCode.Created)
                {
                    var containerResponse = databaseResponse.Database.CreateContainerIfNotExistsAsync(
                            new ContainerProperties
                            {
                                Id = name,
                                PartitionKeyPath = $"/{keyName}"
                            },
                            containerOptions?.ThroughputProperties,
                            containerOptions?.RequestOptions)
                                .ConfigureAwait(false).GetAwaiter().GetResult();
                    if (containerResponse.StatusCode == HttpStatusCode.OK || containerResponse.StatusCode == HttpStatusCode.Created)
                        _containerServices.Add(name, (containerResponse.Container, typeof(T).GetProperties()));
                    else
                        throw new ArgumentException($"It's not possible to create a container with name {name} and key path {keyName}.");
                }
                else
                    throw new ArgumentException($"It's not possible to create a database with name {databaseName}.");
            }
            return this;
        }
    }
}
