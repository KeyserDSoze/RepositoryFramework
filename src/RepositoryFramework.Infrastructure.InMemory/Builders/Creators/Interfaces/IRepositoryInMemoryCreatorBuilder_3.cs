using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryCreatorBuilder<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        IServiceCollection Services { get; }
        IRepositoryInMemoryCreatorBuilder<T, TKey, TState> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex);
        IRepositoryInMemoryCreatorBuilder<T, TKey, TState> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements);
        IRepositoryInMemoryCreatorBuilder<T, TKey, TState> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator);
        IRepositoryInMemoryCreatorBuilder<T, TKey, TState> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start);
        IRepositoryInMemoryCreatorBuilder<T, TKey, TState> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType);
        IRepositoryInMemoryCreatorBuilder<T, TKey, TState> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath);
        IRepositoryInMemoryBuilder<T, TKey, TState> And();
    }
}