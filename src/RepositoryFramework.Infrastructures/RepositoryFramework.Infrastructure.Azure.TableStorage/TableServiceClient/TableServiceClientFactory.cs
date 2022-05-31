using Azure.Data.Tables;

namespace RepositoryFramework.Infrastructure.Azure.TableStorage
{
    public class TableServiceClientFactory
    {
        public static TableServiceClientFactory Instance { get; } = new TableServiceClientFactory();
        private TableServiceClientFactory() { }
        private readonly Dictionary<string, TableClient> _tableServiceClientFactories = new();
        public TableClient Get(string name)
            => _tableServiceClientFactories[name];
        internal TableServiceClientFactory Add(string name, string connectionString)
        {
            if (!_tableServiceClientFactories.ContainsKey(name))
            {
                var serviceClient = new TableServiceClient(connectionString);
                _ = serviceClient.CreateTableIfNotExistsAsync(name).ConfigureAwait(false).GetAwaiter().GetResult();
                var tableClient = new TableClient(connectionString, name);
                _tableServiceClientFactories.Add(name, tableClient);
            }
            else
                throw new ArgumentException($"{name} client already added.");
            return this;
        }
    }
}
