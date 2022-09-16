using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal sealed class MemberExpressionStrategy : IExpressionStrategy
    {
        public string? Convert(Expression expression)
        {
            var memberExpression = (MemberExpression)expression;
            dynamic property = memberExpression.Member;
            if (property.PropertyType == typeof(bool))
            {
                string name = property.Name;
                return $"{name} eq true";
            }
            return null;
        }
    }
}
