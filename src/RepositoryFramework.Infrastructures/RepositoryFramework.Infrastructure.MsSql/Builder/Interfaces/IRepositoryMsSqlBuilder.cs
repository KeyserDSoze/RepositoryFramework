using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.MsSql
{
    public interface IRepositoryMsSqlBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryMsSqlBuilder<T, TKey> WithColumn<TProperty>(Expression<Func<T, TProperty>> property, Action<PropertyHelper<T>> value);
        IRepositoryMsSqlBuilder<T, TKey> WithPrimaryKey<TProperty>(Expression<Func<T, TProperty>> property, Action<PropertyHelper<T>> value);
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}
