using Azure.Data.Tables;
using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class TableStorageRepository<T> : TableStorageRepository<T, string>, IRepositoryPattern<T>
    {
        public TableStorageRepository(TableServiceClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}