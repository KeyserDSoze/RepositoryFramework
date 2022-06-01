using Azure.Data.Tables;
using Azure.Identity;

namespace RepositoryFramework.Infrastructure.Azure.TableStorage
{
    public class TableServiceClientFactory
    {
        public static TableServiceClientFactory Instance { get; } = new TableServiceClientFactory();
        private TableServiceClientFactory() { }
        private readonly Dictionary<string, TableClient> _tableServiceClientFactories = new();
        public TableClient Get(string name)
            => _tableServiceClientFactories[name];
        internal TableServiceClientFactory Add(string name, string connectionString, TableClientOptions? clientOptions)
        {
            var serviceClient = new TableServiceClient(connectionString, clientOptions);
            var tableClient = new TableClient(connectionString, name, clientOptions);
            return Add(name, serviceClient, tableClient);
        }
        internal TableServiceClientFactory Add(string name, Uri endpointUri, TableClientOptions? clientOptions)
        {
            var defaultCredential = new DefaultAzureCredential();
            var serviceClient = new TableServiceClient(endpointUri, defaultCredential, clientOptions);
            var tableClient = new TableClient(endpointUri, name, defaultCredential, clientOptions);
            return Add(name, serviceClient, tableClient);
        }
        private TableServiceClientFactory Add(string name, TableServiceClient serviceClient, TableClient tableClient)
        {
            if (!_tableServiceClientFactories.ContainsKey(name))
            {
                _ = serviceClient.CreateTableIfNotExistsAsync(name).ConfigureAwait(false).GetAwaiter().GetResult();
                _tableServiceClientFactories.Add(name, tableClient);
            }
            return this;
        }
    }
}
