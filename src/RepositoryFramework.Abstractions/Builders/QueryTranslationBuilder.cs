using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

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
        public IQueryTranslationBuilder<T, TKey, TTranslated> WithKey(Expression<Func<TTranslated, TKey>> keyRetriever)
        {
            var compiled = keyRetriever.Compile();
            RepositoryMapper<T, TKey, TTranslated>.Instance.KeyRetriever = compiled;
            return this;
        }
        public IQueryTranslationBuilder<T, TKey, TTranslated> With<TProperty, TTranslatedProperty>(
            Expression<Func<T, TProperty>> property,
            Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            var propertyValue = GetPropertyFromExpression(property)!;
            var translatedPropertyValue = GetPropertyFromExpression(translatedProperty)!;
            var compiledProperty = property.Compile();
            var compiledTranslatedProperty = translatedProperty.Compile();
            RepositoryMapper<T, TKey, TTranslated>.Instance.Properties.Add(
                new RepositoryMapper<T, TKey, TTranslated>.RepositoryMapperProperty(
                    x => compiledProperty.Invoke(x)!,
                    (x, value) => propertyValue.SetValue(x, value),
                    x => compiledTranslatedProperty.Invoke(x)!,
                    (x, value) => translatedPropertyValue.SetValue(x, value)
                    ));
            FilterTranslation.Instance.With(property, translatedProperty);
            return this;
        }
        private static PropertyInfo? GetPropertyFromExpression<Tx, Ty>(Expression<Func<Tx, Ty>> lambda)
        {
            MemberExpression? expression = null;
            if (lambda.Body is UnaryExpression unary)
            {
                if (unary.Operand is MemberExpression member)
                {
                    expression = member;
                }
            }
            else if (lambda.Body is MemberExpression member)
            {
                expression = member;
            }
            return expression?.Member as PropertyInfo;
        }
        public IQueryTranslationBuilder<T, TKey, TFurtherTranslated> AndTranslate<TFurtherTranslated>()
            => Builder.Translate<TFurtherTranslated>();

        public IRepositoryBuilder<T, TKey> Builder => _repositoryBuilder;
        public IServiceCollection Services => _repositoryBuilder.Services;
    }
}
