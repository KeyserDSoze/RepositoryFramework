namespace RepositoryFramework.Web.Components
{
    public sealed class RepositoryUiPropertyValueRetrieved
    {
        public object? Default { get; set; }
        public IEnumerable<PropertyValue>? Values { get; set; }
        public bool IsMultiple { get; set; }
        public Func<object, string>? LabelComparer { get; set; }
    }
}
