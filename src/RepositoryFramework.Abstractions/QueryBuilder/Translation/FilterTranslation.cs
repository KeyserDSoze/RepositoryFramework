using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RepositoryFramework
{
    public sealed class FilterTranslation
    {
        private sealed record TranslationWrapper(Type From, Type To, List<Translation> Translations)
        {
            private readonly ConcurrentDictionary<string, LambdaExpression> _alreadySerialized = new();
            public LambdaExpression? Transform(string? serialized)
            {
                if (string.IsNullOrWhiteSpace(serialized))
                    return null;
                if (!_alreadySerialized.ContainsKey(serialized))
                {
                    var key = serialized;
                    foreach (var translation in Translations)
                    {
                        if (serialized.EndsWith(translation.EndWith))
                        {
                            var place = serialized.LastIndexOf(translation.EndWith);
                            if (place > -1)
                                serialized = serialized.Remove(place, translation.EndWith.Length).Insert(place, translation.Value);
                        }
                        var list = translation.Key.Matches(serialized);
                        for (var i = 0; i < list.Count; i++)
                        {
                            var match = list[i];
                            serialized = serialized.Replace(match.Value, $"{translation.Value}{match.Value.Last()}");
                        }
                    }
                    var deserialized = serialized.DeserializeAsDynamic(To);
                    _alreadySerialized.TryAdd(key, deserialized);
                    return deserialized;
                }
                return _alreadySerialized[serialized];
            }
            public LambdaExpression? Transform(LambdaExpression? from)
                => Transform(from?.Serialize());
        }
        public static FilterTranslation Instance { get; } = new();
        private FilterTranslation() { }
        private sealed record Translation(Regex Key, string EndWith, string Value);
        private readonly Dictionary<string, TranslationWrapper> _translations = new();
        public bool HasTranslation<T>()
            => _translations.ContainsKey(typeof(T).FullName!);
        private static Regex VariableName(string prefix) => new($@"\.{prefix}[^a-zA-Z0-9@_\.]{{1}}");
        public void With<T, TTranslated, TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            var name = typeof(T).FullName!;
            if (!_translations.ContainsKey(name))
                _translations.Add(name, new(typeof(T), typeof(TTranslated), new()));
            var propertyName = string.Join(".", property.ToString().Split('.').Skip(1));
            var translatedPropertyName = $".{string.Join(".", translatedProperty.ToString().Split('.').Skip(1))}";
            _translations[name].Translations.Add(new Translation(VariableName(propertyName), $".{propertyName}", translatedPropertyName));
        }
        public Query Transform<T>(Query transformableQuery)
        {
            var fromType = typeof(T);
            if (!_translations.ContainsKey(fromType.FullName!))
                return transformableQuery;
            var translation = _translations[fromType.FullName!];
            Query query = new();
            foreach (var operation in transformableQuery.Operations)
            {
                if (operation is ValueQueryOperation value)
                    query.Operations.Add(value with { });
                else if (operation is LambdaQueryOperation lambda)
                    query.Operations.Add(new LambdaQueryOperation(lambda.Operation, translation.Transform(lambda.Expression)));
            }
            return query;
        }
        public Query Transform<T>(SerializableQuery serializableQuery)
        {
            var fromType = typeof(T);
            if (!_translations.ContainsKey(fromType.FullName!))
                return serializableQuery.Deserialize<T>();
            var translation = _translations[fromType.FullName!];
            Query query = new();
            foreach (var operation in serializableQuery.Operations)
            {
                if (operation.Operation == QueryOperations.Top || operation.Operation == QueryOperations.Skip)
                {
                    query.Operations.Add(new ValueQueryOperation(
                    operation.Operation, operation.Value != null ? long.Parse(operation.Value) : null));
                }
                else
                {
                    query.Operations.Add(new LambdaQueryOperation(
                    operation.Operation,
                    translation.Transform(operation.Value)));
                }
            }
            return query;
        }
    }
}
