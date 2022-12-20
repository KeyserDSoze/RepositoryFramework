namespace RepositoryFramework.Web.Components
{
    public sealed class BootstrapProperty
    {
        public string Id { get; }
        public string NavigationId { get; }
        public string NavigationSelector { get; }
        public string NavigationTabId { get; }
        public string NavigationTabContentId { get; }
        public BootstrapProperty(string navigationPath)
        {
            var selectorName = navigationPath.ToLower().Replace('.', '_');
            Id = $"id_{selectorName}";
            NavigationId = $"nav_{selectorName}";
            NavigationSelector = $"#{NavigationId}";
            NavigationTabId = $"id_{selectorName}_nav";
            NavigationTabContentId = $"id_{selectorName}_nav_content";
        }
    }
}
