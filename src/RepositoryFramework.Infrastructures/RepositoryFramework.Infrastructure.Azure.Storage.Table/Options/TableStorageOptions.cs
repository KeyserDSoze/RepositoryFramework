using System.Reflection;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public class TableStorageOptions<T, TKey>
        where TKey : notnull
    {
        internal static TableStorageOptions<T, TKey> Instance { get; } = new TableStorageOptions<T, TKey>();
        public PropertyInfo PartitionKey { get; internal set; } = null!;
        public PropertyInfo RowKey { get; internal set; } = null!;
        public PropertyInfo? Timestamp { get; internal set; }
    }
}
