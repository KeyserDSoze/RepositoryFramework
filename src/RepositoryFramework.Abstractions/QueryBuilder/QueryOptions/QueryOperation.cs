using System.Linq.Expressions;

namespace RepositoryFramework
{
    public sealed record QueryOperation(QueryOperations Operation, LambdaExpression? Expression = null, int? Value = null);
}