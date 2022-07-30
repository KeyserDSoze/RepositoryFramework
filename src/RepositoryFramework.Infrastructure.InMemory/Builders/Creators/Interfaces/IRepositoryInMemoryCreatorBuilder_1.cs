using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryCreatorBuilder<T> : IRepositoryInMemoryCreatorBuilder<T, string>
    {
        new IRepositoryInMemoryCreatorBuilder<T> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex);
        new IRepositoryInMemoryCreatorBuilder<T> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements);
        new IRepositoryInMemoryCreatorBuilder<T> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator);
        new IRepositoryInMemoryCreatorBuilder<T> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start);
        new IRepositoryInMemoryCreatorBuilder<T> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType);
        new IRepositoryInMemoryCreatorBuilder<T> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath);
        new IRepositoryInMemoryBuilder<T> And();
    }
}