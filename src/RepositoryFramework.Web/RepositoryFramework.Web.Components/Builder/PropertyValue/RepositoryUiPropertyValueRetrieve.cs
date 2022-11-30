namespace RepositoryFramework.Web.Components
{
    public sealed class RepositoryUiPropertyValueRetrieve
    {
        public object? Default { get; set; }
        public Func<IServiceProvider, Task<IEnumerable<PropertyValue>>>? Retriever { get; set; }
        public bool IsMultiple { get; set; }
        public Func<object, string>? LabelComparer { get; set; }
    }
}
