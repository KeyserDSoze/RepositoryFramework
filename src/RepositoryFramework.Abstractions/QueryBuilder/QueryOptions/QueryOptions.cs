using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Web;

namespace RepositoryFramework
{
    public sealed class QueryOptions<T>
    {
        public Expression<Func<T, bool>>? Predicate { get; internal set; }
        public int? Top { get; internal set; }
        public int? Skip { get; internal set; }
        public List<QueryOrderedOptions<T>> Orders { get; internal set; } = new();
        private const string LogicAnd = "&";
        private const string LogicQuery = "?";
        public string ToQuery()
        {
            var query = new StringBuilder();
            bool isAlreadyAdded = false;
            if (Predicate != null)
            {
                var predicateAsString = Predicate.Serialize();
                query.Append($"{AddSeparator()}query={HttpUtility.UrlEncode(predicateAsString)}");
            }
            if (Top != null)
                query.Append($"{AddSeparator()}top={Top}");
            if (Skip != null)
                query.Append($"{AddSeparator()}skip={Skip}");
            foreach (var order in Orders)
            {
                query.Append($"{AddSeparator()}{order.QuerystringKey}={HttpUtility.UrlEncode(order.Order.Serialize())}");
            }

            return query.ToString();

            string AddSeparator()
            {
                if (!isAlreadyAdded)
                {
                    isAlreadyAdded = true;
                    return LogicQuery;
                }
                else
                    return LogicAnd;
            }
        }
        public static QueryOptions<T> ComposeFromQuery(string? predicateAsString,
            int? top,
            int? skip,
            string? orderAsString,
            string? orderDescendingAsString,
            string[]? thenByAsString,
            string[]? thenByDescendingAsString)
        {
            var options = new QueryOptions<T>();
            if (predicateAsString != null)
                options.Predicate = predicateAsString.Deserialize<T, bool>();
            options.Top = top;
            options.Skip = skip;
            if (orderAsString != null)
                options.Orders.Add(new QueryOrderedOptions<T>(orderAsString.Deserialize<T, object>(), true, false));
            if (orderDescendingAsString != null)
                options.Orders.Add(new QueryOrderedOptions<T>(orderDescendingAsString.Deserialize<T, object>(), false, false));
            if (thenByAsString != null)
                foreach (var thenBy in thenByAsString)
                    options.Orders.Add(new QueryOrderedOptions<T>(thenBy.Deserialize<T, object>(), true, true));
            if (thenByDescendingAsString != null)
                foreach (var thenByDesc in thenByDescendingAsString)
                    options.Orders.Add(new QueryOrderedOptions<T>(thenByDesc.Deserialize<T, object>(), false, true));
            return options;
        }
        public QueryOptionsTranslate<T, TTranslated> Translate<TTranslated>()
            => new(this);
    }
}