using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IQueryTranslationInMemoryBuilder<T, TKey, TTranslated> : IQueryTranslationBuilder<T, TKey, TTranslated>
        where TKey : notnull
    {
        new IQueryTranslationInMemoryBuilder<T, TKey, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty);
        new IRepositoryInMemoryBuilder<T, TKey> Builder { get; }
    }
}