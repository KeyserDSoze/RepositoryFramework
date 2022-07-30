using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryCreatorBuilder<T> : RepositoryInMemoryCreatorBuilder<T, string>, IRepositoryInMemoryCreatorBuilder<T>
    {
        public RepositoryInMemoryCreatorBuilder(RepositoryInMemoryBuilder<T> builder, CreationSettings internalBehaviorSettings) :
            base(builder, internalBehaviorSettings)
        {
        }
        public new IRepositoryInMemoryBuilder<T> And()
            => (IRepositoryInMemoryBuilder<T>)_builder;

        public new IRepositoryInMemoryCreatorBuilder<T> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start)
        {
            _ = base.WithAutoIncrement(navigationPropertyPath, start);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType)
        {
            _ = base.WithImplementation(navigationPropertyPath, implementationType);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            _ = base.WithImplementation<TProperty, TEntity>(navigationPropertyPath);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex)
        {
            _ = base.WithPattern(navigationPropertyPath, regex);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements)
        {
            _ = base.WithSpecificNumberOfElements(navigationPropertyPath, numberOfElements);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator)
        {
            _ = base.WithValue(navigationPropertyPath, creator);
            return this;
        }
    }
}