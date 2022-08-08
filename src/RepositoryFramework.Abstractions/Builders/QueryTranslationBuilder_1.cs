using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class QueryTranslationBuilder<T, TTranslated> : QueryTranslationBuilder<T, string, TTranslated>, IQueryTranslationBuilder<T, TTranslated>
    {
        private readonly IRepositoryBuilder<T> _repositoryBuilder;
        public QueryTranslationBuilder(IRepositoryBuilder<T> repositoryBuilder) : base(repositoryBuilder) 
            => _repositoryBuilder = repositoryBuilder;
        public new IQueryTranslationBuilder<T, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            FilterTranslation.Instance.With(property, translatedProperty);
            return this;
        }
        public new IRepositoryBuilder<T> Builder => _repositoryBuilder;
    }
}