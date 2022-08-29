using RepositoryFramework.Infrastructure.Azure.Storage.Table;
using RepositoryFramework.Test.Domain;

namespace RepositoryFramework.UnitTest.Tests.AllIntegration.TableStorage
{
    internal class TableStorageReader : ITableStorageEntityReader<AppUser, AppUserKey>
    {
        public (string PartitionKey, string RowKey) ReadFromKey(AppUserKey key) 
            => (key.Id.ToString(), string.Empty);
    }
}
