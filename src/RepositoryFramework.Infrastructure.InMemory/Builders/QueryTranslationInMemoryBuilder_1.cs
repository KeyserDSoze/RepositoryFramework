using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    internal class QueryTranslationInMemoryBuilder<T, TTranslated> : QueryTranslationInMemoryBuilder<T, string, TTranslated>, IQueryTranslationInMemoryBuilder<T, TTranslated>
    {
        private readonly IRepositoryInMemoryBuilder<T> _repositoryBuilder;
        public QueryTranslationInMemoryBuilder(IRepositoryInMemoryBuilder<T> repositoryBuilder) : base(repositoryBuilder) 
            => _repositoryBuilder = repositoryBuilder;
        public new IQueryTranslationInMemoryBuilder<T, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            FilterTranslation<T, TTranslated>.Instance.With(property, translatedProperty);
            return this;
        }
        public new IRepositoryInMemoryBuilder<T> Builder => _repositoryBuilder;

        IRepositoryBuilder<T> IQueryTranslationBuilder<T, TTranslated>.Builder
           => _repositoryBuilder;
        IQueryTranslationBuilder<T, TTranslated> IQueryTranslationBuilder<T, TTranslated>.With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
            => With(property, translatedProperty);
    }
}