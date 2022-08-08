using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed record QueryOperationAsString([property:JsonPropertyName("o")] QueryOperations Operation,
        [property: JsonPropertyName("e")] string? Expression = null,
        [property: JsonPropertyName("v")] int? Value = null);
}