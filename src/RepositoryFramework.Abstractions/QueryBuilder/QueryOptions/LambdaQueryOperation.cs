using System.Linq.Expressions;

namespace RepositoryFramework
{
    public record LambdaQueryOperation(QueryOperations Operation, LambdaExpression? Expression) : QueryOperation(Operation);
}