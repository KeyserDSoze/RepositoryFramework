namespace RepositoryFramework.Web.Components.Standard
{
    public sealed class SearchValue
    {
        public object? Value { get; set; }
        public string? Expression { get; set; }
        public required BaseProperty BaseProperty { get; init; }
    }
}
