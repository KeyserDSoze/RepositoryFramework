using Azure.Data.Tables;
using Azure.Identity;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public class TableServiceClientFactory
    {
        public static TableServiceClientFactory Instance { get; } = new TableServiceClientFactory();
        private TableServiceClientFactory() { }
        private readonly Dictionary<string, TableClient> _tableServiceClientFactories = new();
        public TableClient Get(string name)
            => _tableServiceClientFactories[name];
        internal TableServiceClientFactory Add(string name, string tableName, string connectionString, TableClientOptions? clientOptions)
        {
            var serviceClient = new TableServiceClient(connectionString, clientOptions);
            var tableClient = new TableClient(connectionString, tableName, clientOptions);
            return Add(name, serviceClient, tableClient);
        }
        internal TableServiceClientFactory Add(string name, string tableName, Uri endpointUri, TableClientOptions? clientOptions)
        {
            var defaultCredential = new DefaultAzureCredential();
            var serviceClient = new TableServiceClient(endpointUri, defaultCredential, clientOptions);
            var tableClient = new TableClient(endpointUri, tableName, defaultCredential, clientOptions);
            return Add(name, serviceClient, tableClient);
        }
        private TableServiceClientFactory Add(string name, TableServiceClient serviceClient, TableClient tableClient)
        {
            if (!_tableServiceClientFactories.ContainsKey(name))
            {
                _ = serviceClient.CreateTableIfNotExistsAsync(name).ToResult();
                _tableServiceClientFactories.Add(name, tableClient);
            }
            return this;
        }
    }
}
