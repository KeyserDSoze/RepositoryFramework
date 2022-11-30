using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

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
        public IQueryTranslationBuilder<T, TKey, TTranslated> WithKey<TProperty, TTranslatedProperty>(
            Expression<Func<TKey, TProperty>> property,
            Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            var propertyValue = property.GetPropertyFromExpression()!;
            var translatedPropertyValue = translatedProperty.GetPropertyFromExpression()!;
            var compiledProperty = property.Compile();
            var compiledTranslatedProperty = translatedProperty.Compile();
            RepositoryMapper<T, TKey, TTranslated>.Instance.KeyProperties.Add(
               new RepositoryMapper<T, TKey, TTranslated>.RepositoryKeyMapperProperty(
                   translatedPropertyValue,
                   x => compiledProperty.Invoke(x)!,
                   propertyValue != null ? (x, value) => propertyValue.SetValue(x, value) : null,
                   x => compiledTranslatedProperty.Invoke(x)!,
                   (x, value) => translatedPropertyValue.SetValue(x, value)
                   ));
            return this;
        }
        public IQueryTranslationBuilder<T, TKey, TTranslated> With<TProperty, TTranslatedProperty>(
            Expression<Func<T, TProperty>> property,
            Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            var propertyValue = property.GetPropertyFromExpression()!;
            var translatedPropertyValue = translatedProperty.GetPropertyFromExpression()!;
            var compiledProperty = property.Compile();
            var compiledTranslatedProperty = translatedProperty.Compile();
            RepositoryMapper<T, TKey, TTranslated>.Instance.Properties.Add(
                new RepositoryMapper<T, TKey, TTranslated>.RepositoryMapperProperty(
                    x => compiledProperty.Invoke(x)!,
                    (x, value) => propertyValue.SetValue(x, value),
                    x => compiledTranslatedProperty.Invoke(x)!,
                    (x, value) => translatedPropertyValue.SetValue(x, value)
                    ));
            FilterTranslation<T, TKey>.Instance.With(property, translatedProperty);
            return this;
        }
        public IQueryTranslationBuilder<T, TKey, TTranslated> WithSamePorpertiesName()
        {
            var translatedProperties = typeof(TTranslated).GetProperties();
            foreach (var property in typeof(T).GetProperties())
            {
                var translatedProperty = translatedProperties.FirstOrDefault(x => x.Name == property.Name);
                if (translatedProperty != null)
                {
                    RepositoryMapper<T, TKey, TTranslated>.Instance.Properties.Add(
                        new RepositoryMapper<T, TKey, TTranslated>.RepositoryMapperProperty(
                        x => property.GetValue(x)!,
                        (x, value) => property.SetValue(x, value),
                        x => translatedProperty.GetValue(x)!,
                        (x, value) => translatedProperty.SetValue(x, value)
                        ));
                }
            }
            return this;
        }
        public IQueryTranslationBuilder<T, TKey, TFurtherTranslated> AndTranslate<TFurtherTranslated>()
            => Builder.Translate<TFurtherTranslated>();
        public IRepositoryBuilder<T, TKey> Builder => _repositoryBuilder;
        public IServiceCollection Services => _repositoryBuilder.Services;
    }
}
