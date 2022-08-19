using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed record QueryOperationAsString([property:JsonPropertyName("o")] QueryOperations Operation,
        [property: JsonPropertyName("v")] string? Value = null);
}