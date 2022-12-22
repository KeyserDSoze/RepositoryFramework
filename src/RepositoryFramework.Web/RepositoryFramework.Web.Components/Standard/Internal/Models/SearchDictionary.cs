using System.Linq.Expressions;

namespace RepositoryFramework.Web.Components.Standard
{
    public sealed class OrderValue
    {
        public int OrderId { get; set; }
        public LambdaExpression? Expression { get; set; }
        public required BaseProperty BaseProperty { get; init; }
    }
    public sealed class OrderWrapper
    {
        private readonly List<OrderValue> _orders = new();
        private readonly List<OrderValue> _ordersByDescending = new();

    }
    public sealed class SearchWrapper<T>
    {
        private readonly Dictionary<string, SearchValue<T>> _searched = new();
        public SearchValue<T> Get(BaseProperty baseProperty)
        {
            string name = $"{baseProperty.NavigationPath}.{baseProperty.Self.Name}";
            if (!_searched.ContainsKey(name))
                _searched.Add(name, new()
                {
                    BaseProperty = baseProperty
                });
            return _searched[name];
        }
        public IEnumerable<string> GetExpressions()
        {
            foreach (var search in _searched)
                if (search.Value.Expression != null)
                    yield return search.Value.Expression;
        }
        public IEnumerable<Expression<Func<T, bool>>> GetLambdaExpressions()
        {
            foreach (var search in _searched)
                if (search.Value.LambdaExpression != null)
                    yield return search.Value.LambdaExpression;
        }
    }
}
