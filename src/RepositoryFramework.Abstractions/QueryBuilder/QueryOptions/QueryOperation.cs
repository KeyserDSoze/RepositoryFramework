using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed record QueryOperation(QueryOperations Operation, LambdaExpression? Expression = null, int? Value = null);
    public sealed record QueryOperationAsString([property:JsonPropertyName("o")] QueryOperations Operation,
        [property: JsonPropertyName("e")] string? Expression = null,
        [property: JsonPropertyName("v")] int? Value = null);
}