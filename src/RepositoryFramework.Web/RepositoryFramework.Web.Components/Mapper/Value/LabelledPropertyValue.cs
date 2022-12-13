namespace RepositoryFramework.Web
{
    public sealed class LabelledPropertyValue
    {
        public required string Label { get; init; }
        public required string Id { get; init; }
        public required object Value { get; init; }

        public static implicit operator LabelledPropertyValue(string value)
            => new()
            {
                Label = value,
                Id = value,
                Value = value
            };
    }
}
