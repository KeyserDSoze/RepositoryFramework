using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public interface IRepositoryTableStorageBuilder<T> : IRepositoryTableStorageBuilder<T, string>, IRepositoryBuilder<T>
    {
        new IRepositoryTableStorageBuilder<T> WithTableStorageKeyReader<TKeyReader>()
            where TKeyReader : class, ITableStorageKeyReader<T>;
        new IRepositoryTableStorageBuilder<T> WithPartitionKey<TProperty>(
           Expression<Func<T, TProperty>> property);
        new IRepositoryTableStorageBuilder<T> WithRowKey<TProperty>(
           Expression<Func<T, TProperty>> property);
        new IRepositoryTableStorageBuilder<T> WithTimestamp(
           Expression<Func<T, DateTime>> property);
        new IRepositoryTableStorageBuilder<T> WithTimestamp(
           Expression<Func<T, DateTimeOffset>> property);
        new IRepositoryBuilder<T> Builder { get; }
    }
}