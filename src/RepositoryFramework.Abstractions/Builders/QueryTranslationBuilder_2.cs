using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class QueryTranslationBuilder<T, TKey, TTranslated> : IQueryTranslationBuilder<T, TKey, TTranslated>
        where TKey : notnull
    {

        private readonly IRepositoryBuilder<T, TKey> _repositoryBuilder;

        public QueryTranslationBuilder(IRepositoryBuilder<T, TKey> repositoryBuilder)
        {
            _repositoryBuilder = repositoryBuilder;
        }
        public IQueryTranslationBuilder<T, TKey, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            FilterTranslation<T, TTranslated>.Instance.With(property, translatedProperty);
            return this;
        }
        public IRepositoryBuilder<T, TKey> Builder => _repositoryBuilder;
        public IServiceCollection Services => _repositoryBuilder.Services;
    }
}