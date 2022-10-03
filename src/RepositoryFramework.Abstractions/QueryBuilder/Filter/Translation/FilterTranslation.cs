using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RepositoryFramework
{
    public sealed class FilterTranslation
    {
        private sealed record TranslationWrapper(Type From, Type To, List<Translation> Translations)
        {
            public LambdaExpression? Transform(string? serialized)
            {
                if (string.IsNullOrWhiteSpace(serialized))
                    return null;

                foreach (var translation in Translations)
                {
                    if (serialized.EndsWith(translation.EndWith))
                    {
                        var place = serialized.LastIndexOf(translation.EndWith);
                        if (place > -1)
                            serialized = serialized.Remove(place, translation.EndWith.Length).Insert(place, translation.Value);
                    }
                    if (serialized.Contains(translation.InTheMiddleWith))
                    {
                        serialized = serialized.Replace(translation.InTheMiddleWith, translation.ValueInTheMiddle);
                    }
                    var list = translation.Key.Matches(serialized);
                    for (var i = 0; i < list.Count; i++)
                    {
                        var match = list[i];
                        serialized = serialized.Replace(match.Value, $"{translation.Value}{match.Value.Last()}");
                    }
                }
                var deserialized = serialized.DeserializeAsDynamic(To);
                return deserialized;
            }
        }
        public static FilterTranslation Instance { get; } = new();
        private FilterTranslation() { }
        private sealed record Translation(Regex Key, string EndWith, string InTheMiddleWith, string Value, string ValueInTheMiddle);
        private readonly Dictionary<string, Dictionary<string, TranslationWrapper>> _translations = new();
        public bool HasTranslation<T>()
            => _translations.ContainsKey(typeof(T).FullName!);
        private static Regex VariableName(string prefix) => new($@"\.{prefix}[^a-zA-Z0-9@_\.]{{1}}");
        public void With<T, TTranslated, TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty)
        {
            var name = typeof(T).FullName!;
            var translatedName = typeof(TTranslated).FullName!;
            if (!_translations.ContainsKey(name))
                _translations.Add(name, new Dictionary<string, TranslationWrapper>());
            if (!_translations[name].ContainsKey(translatedName))
                _translations[name].Add(translatedName, new(typeof(T), typeof(TTranslated), new()));

            var propertyName = string.Join(".", property.ToString().Split('.').Skip(1));
            var translatedPropertyName = $".{string.Join(".", translatedProperty.ToString().Split('.').Skip(1))}";
            _translations[name][translatedName].Translations.Add(new Translation(VariableName(propertyName), $".{propertyName}", $".{propertyName}.", translatedPropertyName, $"{translatedPropertyName}."));
        }
        public IFilterExpression Transform<T>(SerializableFilter serializableFilter)
        {
            var fromType = typeof(T);
            if (!_translations.ContainsKey(fromType.FullName!))
                return serializableFilter.Deserialize<T>();
            var translations = _translations[fromType.FullName!];
            MultipleFilterExpression multipleFilterExpression = new();
            foreach (var translationKeyValue in translations)
            {
                var translation = translationKeyValue.Value;
                FilterExpression filter = new();
                foreach (var operation in serializableFilter.Operations)
                {
                    if (operation.Operation == FilterOperations.Top || operation.Operation == FilterOperations.Skip)
                    {
                        filter.Operations.Add(new ValueFilterOperation(
                        operation.Operation, operation.Value != null ? long.Parse(operation.Value) : null));
                    }
                    else
                    {
                        filter.Operations.Add(new LambdaFilterOperation(
                        operation.Operation,
                        translation.Transform(operation.Value)));
                    }
                }
                multipleFilterExpression.Filters.Add(translationKeyValue.Key, filter);
            }
            return multipleFilterExpression;
        }
    }
}
