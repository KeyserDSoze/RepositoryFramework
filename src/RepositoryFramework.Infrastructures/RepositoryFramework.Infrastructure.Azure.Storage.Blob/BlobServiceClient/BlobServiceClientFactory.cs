using Azure.Identity;
using Azure.Storage.Blobs;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Blob
{
    public class BlobServiceClientFactory
    {
        public static BlobServiceClientFactory Instance { get; } = new BlobServiceClientFactory();
        private BlobServiceClientFactory() { }
        private readonly Dictionary<string, BlobContainerClient> _containerClientFactories = new();
        public BlobContainerClient Get(string name)
            => _containerClientFactories[name];
        internal BlobServiceClientFactory Add(string name, string containerName, string connectionString,  BlobClientOptions? clientOptions)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName.ToLower(), clientOptions);
            return Add(name, containerClient);
        }
        internal BlobServiceClientFactory Add(string name, string containerName, Uri endpointUri, BlobClientOptions? clientOptions)
        {
            var defaultCredential = new DefaultAzureCredential();
            var containerClient = new BlobContainerClient(endpointUri, defaultCredential, clientOptions);
            return Add(name, containerClient);
        }
        private BlobServiceClientFactory Add(string name, BlobContainerClient containerClient)
        {
            if (!_containerClientFactories.ContainsKey(name))
            {
                _ = containerClient.CreateIfNotExistsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                _containerClientFactories.Add(name, containerClient);
            }
            return this;
        }
    }
}
