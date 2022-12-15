using System.Linq.Expressions;

namespace RepositoryFramework.Web.Components.Standard
{
    public sealed class SearchDictionary
    {
        private readonly Dictionary<string, SearchValue> _searched = new();
        public SearchValue Get(BaseProperty baseProperty)
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
        public IEnumerable<Expression<Func<T, bool>>> GetExpressions<T>()
        {
            foreach (var expression in GetExpressions())
                yield return expression.Deserialize<T, bool>();
        }
    }
}
