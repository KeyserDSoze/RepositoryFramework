namespace RepositoryFramework.Web
{
    public sealed class PropertyUiValue
    {
        public object? Default { get; init; }
        public IEnumerable<LabelledPropertyValue>? Values { get; init; }
        public bool IsMultiple { get; init; }
        public bool HasTextEditor { get; init; }
        public Func<object, string>? LabelComparer { get; init; }
    }
}
