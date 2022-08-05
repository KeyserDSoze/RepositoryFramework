using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed record QueryOptions([property: JsonPropertyName("p")] string? Predicate,
        [property: JsonPropertyName("t")] int? Top,
        [property: JsonPropertyName("s")] int? Skip,
        [property: JsonPropertyName("o")] List<QueryOrderedOptions> Orders,
        [property: JsonPropertyName("a")] string? AggregationPredicate)
    {
        public static QueryOptions Empty { get; } = new(null, null, null, new(), null);
        public static string EmptyAsJson { get; } = Empty.ToJson();
        public QueryOptions<T> Transform<T>()
        {
            var options = new QueryOptions<T>();
            if (Predicate != null)
                options.Predicate = Predicate.Deserialize<T, bool>();
            options.Top = Top;
            options.Skip = Skip;
            foreach (var order in Orders)
                options.Orders.Add(new(order.Order.Deserialize<T, object>(), order.IsAscending, order.ThenBy));
            return options;
        }
    }
    public sealed record QueryOrderedOptions(
        [property: JsonPropertyName("o")] string Order,
        [property: JsonPropertyName("a")] bool IsAscending,
        [property: JsonPropertyName("t")] bool ThenBy);
    public sealed class QueryOptions<T>
    {
        public static QueryOptions<T> Empty { get; } = new();
        public Expression<Func<T, bool>>? Predicate { get; internal set; }
        public int? Top { get; internal set; }
        public int? Skip { get; internal set; }
        public List<QueryOrderedOptions<T>> Orders { get; internal set; } = new();
        public string ToBodyAsJson(LambdaExpression? aggregateExpression = null)
        {
            var query = new QueryOptions(
                Predicate?.Serialize(),
                Top,
                Skip,
                Orders.Select(order => new QueryOrderedOptions(order.Order.Serialize(), order.IsAscending, order.ThenBy)).ToList(),
                aggregateExpression?.Serialize()
                );

            return query.ToJson();
        }
        public QueryOptions<TTranslated>? Translate<TTranslated>()
            => FilterTranslation<T, TTranslated>.Instance.Execute(this);
    }
}