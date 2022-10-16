using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    internal class QueryTranslationInMemoryBuilder<T, TKey, TTranslated> : IQueryTranslationInMemoryBuilder<T, TKey, TTranslated>
        where TKey : notnull
    {
        private readonly IRepositoryInMemoryBuilder<T, TKey> _repositoryBuilder;
        public QueryTranslationInMemoryBuilder(IRepositoryInMemoryBuilder<T, TKey> repositoryBuilder)
        {
            _repositoryBuilder = repositoryBuilder;
        }
        public IQueryTranslationInMemoryBuilder<T, TKey, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            FilterTranslation.Instance.With(property, translatedProperty);
            return this;
        }
        public IRepositoryInMemoryBuilder<T, TKey> Builder => _repositoryBuilder;
        public IServiceCollection Services => _repositoryBuilder.Services;
        IRepositoryBuilder<T, TKey> IQueryTranslationBuilder<T, TKey, TTranslated>.Builder
            => _repositoryBuilder;
        IQueryTranslationBuilder<T, TKey, TTranslated> IQueryTranslationBuilder<T, TKey, TTranslated>.With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
            => With(property, translatedProperty);
        public IQueryTranslationBuilder<T, TKey, TFurtherTranslated> AndTranslate<TFurtherTranslated>()
            => _repositoryBuilder.Translate<TFurtherTranslated>();

        public IQueryTranslationBuilder<T, TKey, TTranslated> WithKey(Expression<Func<TTranslated, TKey>> keyRetriever) 
            => this;
    }
}
