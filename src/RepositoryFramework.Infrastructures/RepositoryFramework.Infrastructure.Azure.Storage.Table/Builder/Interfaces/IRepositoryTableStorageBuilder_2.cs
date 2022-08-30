using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public interface IRepositoryTableStorageBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryTableStorageBuilder<T, TKey> WithTableStorageKeyReader<TKeyReader>()
            where TKeyReader : class, ITableStorageKeyReader<T, TKey>;
        IRepositoryTableStorageBuilder<T, TKey> WithPartitionKey<TProperty>(
           Expression<Func<T, TProperty>> property);
        IRepositoryTableStorageBuilder<T, TKey> WithRowKey<TProperty>(
           Expression<Func<T, TProperty>> property);
        IRepositoryTableStorageBuilder<T, TKey> WithTimestamp(
           Expression<Func<T, DateTime>> property);
        IRepositoryTableStorageBuilder<T, TKey> WithTimestamp(
           Expression<Func<T, DateTimeOffset>> property);
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}