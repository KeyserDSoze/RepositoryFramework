using System.Linq.Expressions;
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
                options.Where = Predicate.Deserialize<T, bool>();
            options.Top = Top;
            options.Skip = Skip;
            foreach (var order in Orders)
                options.Orders.Add(new(order.Order.Deserialize<T, object>(), order.IsAscending, order.ThenBy));
            if (Select != null)
                options.Select = Select.DeserializeAsDynamic<T>();
            if (Aggregate != null)
                options.Aggregate = Aggregate.DeserializeAsDynamic<T>();
            return options;
        }
    }
}