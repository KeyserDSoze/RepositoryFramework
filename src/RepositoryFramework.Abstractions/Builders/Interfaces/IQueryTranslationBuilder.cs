using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Translation builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    /// <typeparam name="TStorage">Storage for your repository.</typeparam>
    /// <typeparam name="TTranslated">Model for translation, T to TTranslated and viceversa.</typeparam>
    public interface IQueryTranslationBuilder<T, TKey, out TStorage, TTranslated>
        where TKey : notnull
        where TStorage : class
    {
        IRepositoryBuilder<T, TKey, TStorage> Builder { get; }
        IServiceCollection Services { get; }
        IQueryTranslationBuilder<T, TKey, TStorage, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty);
        IQueryTranslationBuilder<T, TKey, TStorage, TTranslated> WithSamePorpertiesName();
        IQueryTranslationBuilder<T, TKey, TStorage, TTranslated> WithKey<TProperty, TTranslatedProperty>(
            Expression<Func<TKey, TProperty>> property,
            Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty);
        IQueryTranslationBuilder<T, TKey, TStorage, TFurtherTranslated> AndTranslate<TFurtherTranslated>();
    }
}
