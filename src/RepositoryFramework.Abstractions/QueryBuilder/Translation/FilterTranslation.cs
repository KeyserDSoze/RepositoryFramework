using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RepositoryFramework
{
    public sealed class FilterTranslation
    {
        private sealed record TranslationWrapper(Type From, Type To, List<Translation> Translations)
        {
            private readonly ConcurrentDictionary<string, LambdaExpression> AlreadySerialized = new();
            public LambdaExpression? Transform(string? serialized)
            {
                if (string.IsNullOrWhiteSpace(serialized))
                    return null;
                if (!AlreadySerialized.ContainsKey(serialized))
                {
                    string key = serialized;
                    foreach (var translation in Translations)
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
                    var deserialized = serialized.DeserializeAsDynamic(To);
                    AlreadySerialized.TryAdd(key, deserialized);
                    return deserialized;
                }
                return AlreadySerialized[serialized];
            }
            public LambdaExpression? Transform(LambdaExpression? from) 
                => Transform(from?.Serialize());
        }
        public static FilterTranslation Instance { get; } = new();
        private FilterTranslation() { }
        private sealed record Translation(Regex Key, string EndWith, string Value);
        private readonly Dictionary<string, TranslationWrapper> _translations = new();
        private static Regex VariableName(string prefix) => new($@".{prefix}[^a-zA-Z0-9@_\.]{{1}}");
        public void With<T, TTranslated, TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            string name = typeof(T).FullName!;
            if (!_translations.ContainsKey(name))
                _translations.Add(name, new(typeof(T), typeof(TTranslated), new()));
            string propertyName = string.Join(".", property.ToString().Split('.').Skip(1));
            string translatedPropertyName = $".{string.Join(".", translatedProperty.ToString().Split('.').Skip(1))}";
            _translations[name].Translations.Add(new Translation(VariableName(propertyName), $".{propertyName}", translatedPropertyName));
        }
        public Query Transform<T>(Query _query)
        {
            Type fromType = typeof(T);
            if (!_translations.ContainsKey(fromType.FullName!))
                return _query;
            TranslationWrapper translation = _translations[fromType.FullName!];
            Query query = new();
            foreach (var operation in _query.Operations)
            {
                query.Operations.Add(new QueryOperation(
                    operation.Operation,
                    translation.Transform(operation.Expression),
                    operation.Value));
            }
            return query;
        }
        public Query Transform<T>(SerializableQuery _query)
        {
            Type fromType = typeof(T);
            if (!_translations.ContainsKey(fromType.FullName!))
                return _query.Deserialize<T>();
            TranslationWrapper translation = _translations[fromType.FullName!];
            Query query = new();
            foreach (var operation in _query.Operations)
            {
                query.Operations.Add(new QueryOperation(
                    operation.Operation,
                    translation.Transform(operation.Expression),
                    operation.Value));
            }
            return query;
        }
    }
}