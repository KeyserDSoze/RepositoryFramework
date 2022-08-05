using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IQueryTranslationInMemoryBuilder<T, TTranslated> : IQueryTranslationBuilder<T, TTranslated>
    {
        new IQueryTranslationInMemoryBuilder<T, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty);
        new IRepositoryInMemoryBuilder<T> Builder { get; }
    }
}