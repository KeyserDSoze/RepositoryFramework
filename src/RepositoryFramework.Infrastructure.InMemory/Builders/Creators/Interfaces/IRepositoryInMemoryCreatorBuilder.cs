using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryCreatorBuilder<T, TKey>
        where TKey : notnull
    {
        IServiceCollection Services { get; }
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<IServiceProvider, Task<TProperty>> valueRetriever);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithRandomValue<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationPropertyPath,
            Func<IServiceProvider, Task<IEnumerable<TProperty>>> valuesRetriever);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithRandomValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath,
            Func<IServiceProvider, Task<IEnumerable<TProperty>>> valuesRetriever);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType);
        IRepositoryInMemoryCreatorBuilder<T, TKey> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath);
        IRepositoryInMemoryBuilder<T, TKey> And();
    }
}
