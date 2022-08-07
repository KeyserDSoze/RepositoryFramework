using System.Linq.Expressions;

namespace RepositoryFramework
{
    public class QueryOptions<T>
    {
        public static QueryOptions<T> Empty { get; } = new();
        public Expression<Func<T, bool>>? Where { get; internal set; }
        public int? Top { get; internal set; }
        public int? Skip { get; internal set; }
        public List<QueryOrderedOptions<T>> Orders { get; internal set; } = new();
        public LambdaExpression? Select { get; internal set; }
        public LambdaExpression? Aggregate { get; internal set; }
        public QueryOptions ToBody()
        {
            var query = new QueryOptions(
                Where?.Serialize(),
                Top,
                Skip,
                Orders.Select(order => new QueryOrderedOptions(order.Order.Serialize(), order.IsAscending, order.ThenBy)).ToList(),
                Select?.Serialize(),
                Aggregate?.Serialize());

            return query;
        }
#warning test to key
        public string ToKey()
            => ToBody().ToString();
        public QueryOptions<TTranslated>? Translate<TTranslated>()
            => FilterTranslation<T, TTranslated>.Instance.Execute(this);
    }
}