namespace RepositoryFramework.Web
{
    public sealed class LabelledPropertyValue
    {
        public string Label { get; set; } = null!;
        public object Value { get; set; } = null!;
        public static implicit operator LabelledPropertyValue(string value)
            => new()
            {
                Label = value,
                Value = value
            };
    }
}
