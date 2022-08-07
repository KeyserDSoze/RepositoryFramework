using System.Text.Json.Serialization;

namespace RepositoryFramework
{
#warning AR move to another file these classes/records
    public sealed record QueryOrderedOptions(
        [property: JsonPropertyName("o")] string Order,
        [property: JsonPropertyName("a")] bool IsAscending,
        [property: JsonPropertyName("t")] bool ThenBy);
}