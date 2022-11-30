namespace RepositoryFramework.Web.Components
{
    public sealed class PropertyValue
    {
        public string Label { get; set; }
        public object Value { get; set; }
        public static implicit operator PropertyValue(string value)
        {
            return new()
            {
                Label = value,
                Value = value
            };
        }
    }
}
