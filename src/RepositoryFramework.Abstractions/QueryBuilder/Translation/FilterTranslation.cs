using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RepositoryFramework
{
    public sealed class FilterTranslation<T, TTranslated>
    {
        public static FilterTranslation<T, TTranslated> Instance { get; } = new();
        private FilterTranslation() { }
        private sealed record Translation(Regex Key, string EndWith, string Value);
        private readonly List<Translation> _translations = new();
        private static Regex VariableName(string prefix) => new($@".{prefix}[^a-zA-Z0-9@_\.]{{1}}");
        public void With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            string propertyName = string.Join(".", property.ToString().Split('.').Skip(1));
            string translatedPropertyName = $".{string.Join(".", translatedProperty.ToString().Split('.').Skip(1))}";
            _translations.Add(new Translation(VariableName(propertyName), $".{propertyName}", translatedPropertyName));
        }
        public QueryOptions<TTranslated>? Execute(QueryOptions<T>? _queryOptions)
        {
            if (_queryOptions == null)
                return null;
            var translatedQueryOptions = new QueryOptions<TTranslated>()
            {
                Skip = _queryOptions.Skip,
                Top = _queryOptions.Top,
            };
            if (_queryOptions.Where != null)
            {
                var serializedPredicate = _queryOptions.Where.Serialize();
                translatedQueryOptions.Where = ReplaceWithValues(serializedPredicate).Deserialize<TTranslated, bool>();
            }
            if (_queryOptions.Orders != null)
            {
                foreach (var order in _queryOptions.Orders)
                {
                    translatedQueryOptions.Orders.Add(new(
                        ReplaceWithValues(order.Order.Serialize()).Deserialize<TTranslated, object>(),
                        order.IsAscending,
                        order.ThenBy));
                }
            }
            return translatedQueryOptions;
        }
        private readonly ConcurrentDictionary<string, string> AlreadySerialized = new();
        private string ReplaceWithValues(string serialized)
        {
            if (!AlreadySerialized.ContainsKey(serialized))
            {
                string key = serialized;
                foreach (var translation in _translations)
                {
                    if (serialized.EndsWith(translation.EndWith))
                    {
                        int place = serialized.LastIndexOf(translation.EndWith);
                        if (place > -1)
                            serialized = serialized.Remove(place, translation.EndWith.Length).Insert(place, translation.Value);
                    }
                    var list = translation.Key.Matches(serialized);
                    for (int i = 0; i < list.Count; i++)
                    {
                        Match match = list[i];
                        serialized = serialized.Replace(match.Value, $"{translation.Value}{match.Value.Last()}");
                    }
                }
                AlreadySerialized.TryAdd(key, serialized);
                return serialized;
            }
            return AlreadySerialized[serialized];
        }
    }
}