using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace RepositoryFramework
{
    public sealed class QueryOptions<T>
    {
        public Expression<Func<T, bool>>? Predicate { get; internal set; }
        public int? Top { get; internal set; }
        public int? Skip { get; internal set; }
        public Expression<Func<T, object>>? Order { get; internal set; }
        public bool IsAscending { get; internal set; }
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
            if (Order != null)
            {
                var orderAsString = Order.Serialize();
                query.Append($"{AddSeparator()}order={HttpUtility.UrlEncode(orderAsString)}&asc={IsAscending}");
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
            bool? isAscending)
        {
            var options = new QueryOptions<T>();
            if (predicateAsString != null)
                options.Predicate = predicateAsString.Deserialize<T, bool>();
            if (top != null)
                options.Top = top;
            if (skip != null)
                options.Skip = skip;
            if (orderAsString != null)
                options.Order = orderAsString.Deserialize<T, object>();
            if (isAscending != null)
                options.IsAscending = isAscending.HasValue && isAscending.Value;
            
            return options;
        }
    }
}