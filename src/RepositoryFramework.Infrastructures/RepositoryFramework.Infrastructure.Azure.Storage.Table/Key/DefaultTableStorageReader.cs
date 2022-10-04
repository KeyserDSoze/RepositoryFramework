using System.Reflection;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal sealed class DefaultTableStorageKeyReader<T, TKey> : ITableStorageKeyReader<T, TKey>
        where TKey : notnull
    {
        public (string PartitionKey, string RowKey) Read(TKey key)
            => (key?.ToString() ?? string.Empty, string.Empty);
        public TKey Read(T entity)
            => Constructor.InvokeWithBestDynamicFit<TKey>(
                entity!.GetType().FetchProperties()
                    .Where(x => x.PropertyType == typeof(string)).Take(2).ToArray())!;
    }
}
