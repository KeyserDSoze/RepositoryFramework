using System.Linq.Expressions;

namespace RepositoryFramework
{
    public class QueryOptionsTranslate<T, TTranslated>
    {
        private readonly Dictionary<string, string> _translations = new();
        private readonly QueryOptions<T> _queryOptions;
        public QueryOptionsTranslate(QueryOptions<T> queryOptions)
        {
            _queryOptions = queryOptions;
        }
        public QueryOptionsTranslate<T, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            string propertyName = $".{string.Join(".", property.ToString().Split('.').Skip(1))}";
            string translatedPropertyName = $".{string.Join(".", translatedProperty.ToString().Split('.').Skip(1))}";
            if (!_translations.ContainsKey(propertyName))
                _translations.Add(propertyName, translatedPropertyName);
            return this;
        }
        private QueryOptions<TTranslated> Execute()
        {
            var queryOptions = new QueryOptions<TTranslated>()
            {
                IsAscending = _queryOptions.IsAscending,
                Skip = _queryOptions.Skip,
                Top = _queryOptions.Top,
            };
            if (_queryOptions.Predicate != null)
            {
                var serializedPredicate = _queryOptions.Predicate.Serialize();
                queryOptions.Predicate = ReplaceWithValues(serializedPredicate).Deserialize<TTranslated, bool>();
            }
            if (_queryOptions.Order != null)
            {
                var serializedOrder = _queryOptions.Order.Serialize();
                queryOptions.Order = ReplaceWithValues(serializedOrder).Deserialize<TTranslated, object>();
            }
            return queryOptions;
        }
        private string ReplaceWithValues(string serialized)
        {
            foreach (var translation in _translations
                .OrderByDescending(x => x.Key.Split('.').Length)
                .Where(translation => serialized.Contains(translation.Key)))
                serialized = serialized.Replace(translation.Key, translation.Value);
            return serialized;
        }
        public static implicit operator QueryOptions<TTranslated>?(QueryOptionsTranslate<T, TTranslated>? options) 
            => options?.Execute();
    }
}