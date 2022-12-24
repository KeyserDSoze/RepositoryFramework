using System.Linq.Expressions;

namespace RepositoryFramework.Web.Components.Standard
{
    public sealed class OrderValue<T, TKey>
        where TKey : notnull
    {
        public required bool ByDescending { get; init; }
        public required string Expression { get; init; }
        public required Expression<Func<T, object>> LambdaExpression { get; init; }
        public required BaseProperty BaseProperty { get; init; }
    }
}
