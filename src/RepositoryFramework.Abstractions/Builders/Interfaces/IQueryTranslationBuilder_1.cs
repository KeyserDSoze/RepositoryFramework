using System.Linq.Expressions;

namespace RepositoryFramework
{
    public interface IQueryTranslationBuilder<T, TTranslated> : IQueryTranslationBuilder<T, string, TTranslated>
    {
        new IRepositoryBuilder<T> Builder { get; }
        new IQueryTranslationBuilder<T, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty);
    }
}