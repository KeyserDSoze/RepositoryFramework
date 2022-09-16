using Azure.Storage.Blobs;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Blob
{
    internal sealed class BlobStorageRepository<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly BlobContainerClient _client;
        public BlobStorageRepository(BlobServiceClientFactory clientFactory)
        {
            _client = clientFactory.Get(typeof(T).Name);
        }
        public async Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteBlobAsync(key!.ToString(), cancellationToken: cancellationToken).NoContext();
            return IState.Default<T>(!response.IsError);
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            if (await blobClient.ExistsAsync(cancellationToken).NoContext())
            {
                var blobData = await blobClient.DownloadContentAsync(cancellationToken).NoContext();
                return JsonSerializer.Deserialize<T>(blobData.Value.Content);
            }
            return default;
        }
        public async Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            return IState.Default<T>(await blobClient.ExistsAsync(cancellationToken).NoContext());
        }

        public async Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            var entityWithKey = IEntity.Default(key, value);
            var response = await blobClient.UploadAsync(new BinaryData(entityWithKey.ToJson()), cancellationToken).NoContext();
            return IState.Default<T>(response.Value != null, value);
        }

        public async IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(Query query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            Func<T, bool> predicate = x => true;
#warning to check well, check a new way to create the query
            var where = (query.Operations.FirstOrDefault(x => x.Operation == QueryOperations.Where) as LambdaQueryOperation)?.Expression;
            if (where != null)
                predicate = where.AsExpression<T, bool>().Compile();
            await foreach (var blob in _client.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var blobClient = _client.GetBlobClient(blob.Name);
                var blobData = await blobClient.DownloadContentAsync(cancellationToken).NoContext();
                var item = JsonSerializer.Deserialize<IEntity<T, TKey>>(blobData.Value.Content)!;
                if (!predicate.Invoke(item.Value))
                    continue;
                yield return item;
            }
        }

        public async Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            var blobClient = _client.GetBlobClient(key!.ToString());
            var response = await blobClient.UploadAsync(new BinaryData(JsonSerializer.Serialize(value)), true, cancellationToken).NoContext();
            return IState.Default<T>(response.Value != null, value);
        }

        public async ValueTask<TProperty> OperationAsync<TProperty>(
         OperationType<TProperty> operation,
         Query query,
         CancellationToken cancellationToken = default)
        {
#warning to refactor
            List<T> items = new();
            await foreach (var item in QueryAsync(query, cancellationToken))
                items.Add(item.Value);
            var select = query.FirstSelect;
            return (await operation.ExecuteAsync(
                () => items.Count,
                null!,
                () => items.Select(x => select!.InvokeAndTransform<object>(x!)).Max(),
                () => items.Select(x => select!.InvokeAndTransform<object>(x!)).Min(),
                null!))!;
        }
        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            BatchResults<T, TKey> results = new();
            foreach (var operation in operations.Values)
            {
                switch (operation.Command)
                {
                    case CommandType.Delete:
                        results.AddDelete(operation.Key, await DeleteAsync(operation.Key, cancellationToken).NoContext());
                        break;
                    case CommandType.Insert:
                        results.AddInsert(operation.Key, await InsertAsync(operation.Key, operation.Value!, cancellationToken).NoContext());
                        break;
                    case CommandType.Update:
                        results.AddUpdate(operation.Key, await UpdateAsync(operation.Key, operation.Value!, cancellationToken).NoContext());
                        break;
                }
            }
            return results;
        }
    }
}
