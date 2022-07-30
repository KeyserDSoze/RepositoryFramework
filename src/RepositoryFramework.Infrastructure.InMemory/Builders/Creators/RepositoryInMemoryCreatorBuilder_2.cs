using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryCreatorBuilder<T, TKey> : RepositoryInMemoryCreatorBuilder<T, TKey, State<T>>, IRepositoryInMemoryCreatorBuilder<T, TKey>
        where TKey : notnull
    {
        public RepositoryInMemoryCreatorBuilder(RepositoryInMemoryBuilder<T, TKey> builder, CreationSettings internalBehaviorSettings) :
            base(builder, internalBehaviorSettings)
        {
        }
        public new IRepositoryInMemoryBuilder<T, TKey> And()
            => (IRepositoryInMemoryBuilder<T, TKey>)_builder;
        public new IRepositoryInMemoryCreatorBuilder<T, TKey> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start)
        {
            _ = base.WithAutoIncrement(navigationPropertyPath, start);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T, TKey> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType)
        {
            _ = base.WithImplementation(navigationPropertyPath, implementationType);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T, TKey> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            _ = base.WithImplementation<TProperty, TEntity>(navigationPropertyPath);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T, TKey> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex)
        {
            _ = base.WithPattern(navigationPropertyPath, regex);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T, TKey> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements)
        {
            _ = base.WithSpecificNumberOfElements(navigationPropertyPath, numberOfElements);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T, TKey> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator)
        {
            _ = base.WithValue(navigationPropertyPath, creator);
            return this;
        }
    }
}