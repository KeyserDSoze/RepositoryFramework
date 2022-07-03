using Azure.Storage.Blobs;
using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Blob
{
    internal class BlobStorageRepository<T> : BlobStorageRepository<T, string>, IRepositoryPattern<T>, IQueryPattern<T>, ICommandPattern<T>
    {
        public BlobStorageRepository(BlobServiceClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}