using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed record QueryOperationAsString([property:JsonPropertyName("q")] QueryOperations Operation,
        [property: JsonPropertyName("v")] string? Value = null);
}