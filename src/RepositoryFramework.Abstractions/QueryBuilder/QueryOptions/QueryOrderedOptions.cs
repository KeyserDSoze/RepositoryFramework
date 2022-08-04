using System.Linq.Expressions;

namespace RepositoryFramework
{
    public sealed record QueryOrderedOptions<T>(Expression<Func<T, object>> Order, bool IsAscending, bool ThenBy)
    {
        public string QuerystringKey => $"{(ThenBy ? "thenBy" : "orderBy")}{(IsAscending ? string.Empty : "Desc")}";
    }
}