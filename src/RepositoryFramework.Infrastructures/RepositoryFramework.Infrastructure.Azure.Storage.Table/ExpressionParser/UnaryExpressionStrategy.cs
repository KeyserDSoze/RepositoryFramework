using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal sealed class UnaryExpressionStrategy : IExpressionStrategy
    {
        public string? Convert(Expression expression)
        {
            var unaryExpression = (UnaryExpression)expression;
            dynamic operand = unaryExpression.Operand;
            if (operand.Member.PropertyType == typeof(bool))
                return $"{operand.Member.Name} eq false";
            return null;
        }
    }
}
