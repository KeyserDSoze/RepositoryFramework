using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class ExpressionParser
    {
        private readonly Expression _expression;
        public ExpressionParser(Expression expression)
        {
            _expression = expression;
        }
        public string ToFilter()
        {
            //string partitionKey =
            return _expression.ToString();
        }
    }
}
