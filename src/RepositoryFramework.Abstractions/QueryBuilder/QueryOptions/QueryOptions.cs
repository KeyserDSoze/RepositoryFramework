using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed record QueryOptions([property: JsonPropertyName("p")] string? Predicate,
        [property: JsonPropertyName("t")] int? Top,
        [property: JsonPropertyName("s")] int? Skip,
        [property: JsonPropertyName("o")] List<QueryOrderedOptions> Orders,
        [property: JsonPropertyName("l")] string? Select,
        [property: JsonPropertyName("a")] string? Aggregate)
    {
        public static QueryOptions Empty { get; } = new(null, null, null, new(), null, null);
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
    public class QueryOptions<T, TProperty> : QueryOptions<T>
    {
        public new Expression<Func<T, TProperty>>? Aggregate { get; internal set; }
    }
    public class QueryOptions<T>
    {
        public static QueryOptions<T> Empty { get; } = new();
        public Expression<Func<T, bool>>? Predicate { get; internal set; }
        public int? Top { get; internal set; }
        public int? Skip { get; internal set; }
        public List<QueryOrderedOptions<T>> Orders { get; internal set; } = new();
        public Expression<Func<T, object>>? Select { get; internal set; }
        public Expression<Func<T, object>>? Aggregate { get; internal set; }
        public QueryOptions ToBody()
        {
            var query = new QueryOptions(
                Predicate?.Serialize(),
                Top,
                Skip,
                Orders.Select(order => new QueryOrderedOptions(order.Order.Serialize(), order.IsAscending, order.ThenBy)).ToList(),
                Select?.Serialize(),
                Aggregate?.Serialize());

            return query;
        }
        public QueryOptions<TTranslated>? Translate<TTranslated>()
            => FilterTranslation<T, TTranslated>.Instance.Execute(this);
    }
}