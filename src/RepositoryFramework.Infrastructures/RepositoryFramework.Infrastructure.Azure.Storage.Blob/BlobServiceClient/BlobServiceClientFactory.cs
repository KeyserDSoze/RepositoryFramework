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
        internal BlobServiceClientFactory Add(string containerName, string connectionString,  BlobClientOptions? clientOptions)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName.ToLower(), clientOptions);
            return Add(containerName, containerClient);
        }
        internal BlobServiceClientFactory Add(string containerName, Uri endpointUri, BlobClientOptions? clientOptions)
        {
            var defaultCredential = new DefaultAzureCredential();
            var containerClient = new BlobContainerClient(endpointUri, defaultCredential, clientOptions);
            return Add(containerName, containerClient);
        }
        private BlobServiceClientFactory Add(string containerName, BlobContainerClient containerClient)
        {
            if (!_containerClientFactories.ContainsKey(containerName))
            {
                _ = containerClient.CreateIfNotExistsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                _containerClientFactories.Add(containerName, containerClient);
            }
            return this;
        }
    }
}
