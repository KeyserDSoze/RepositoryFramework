using Azure.Storage.Blobs;
using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Blob
{
    internal sealed class BlobStorageRepository<T, TKey> : IRepositoryPattern<T, TKey>, IQueryPattern<T, TKey>, ICommandPattern<T, TKey>
        where TKey : notnull
    {
        private readonly BlobContainerClient _client;
        public BlobStorageRepository(BlobServiceClientFactory clientFactory)
        {
            _client = clientFactory.Get(typeof(T).Name);
        }
        public async Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteBlobAsync(key!.ToString(), cancellationToken: cancellationToken);
            return !response.IsError;
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            if (await blobClient.ExistsAsync(cancellationToken))
            {
                var blobData = await blobClient.DownloadContentAsync(cancellationToken);
                return JsonSerializer.Deserialize<T>(blobData.Value.Content);
            }
            return default;
        }
        public async Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            return await blobClient.ExistsAsync(cancellationToken);
        }

        public async Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            var response = await blobClient.UploadAsync(new BinaryData(JsonSerializer.Serialize(value)), cancellationToken);
            return response.Value != null;
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            List<T> items = new();
            await foreach (var blob in _client.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                var blobClient = _client.GetBlobClient(blob.Name);
                var blobData = await blobClient.DownloadContentAsync(cancellationToken);
                items.Add(JsonSerializer.Deserialize<T>(blobData.Value.Content)!);
            }
            IEnumerable<T> results = items;
            if (predicate != null)
                results = results.Where(predicate.Compile());
            if (top != null)
                results = results.Take(top.Value);
            if (skip != null)
                results = results.Skip(skip.Value);
            return results;
        }

        public async Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            var response = await blobClient.UploadAsync(new BinaryData(JsonSerializer.Serialize(value)), true, cancellationToken);
            return response.Value != null;
        }
    }
}