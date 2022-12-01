namespace RepositoryFramework.Web.Components
{
    public sealed class AppMenu
    {
        public AppMenu(RepositoryFrameworkRegistry repositoryFrameworkRegistry, IServiceProvider serviceProvider)
        {
            Models = new();
            foreach (var service in repositoryFrameworkRegistry.Services)
            {
                if (!AppInternalSettings.Instance.NotExposableRepositories.Any(x => x.ToLower() == service.ModelType.Name.ToLower())
                    && !Models.ContainsKey(service.ModelType.Name.ToLower()))
                    Models.Add(service.ModelType.Name.ToLower(), service);
            }
        }
        public Dictionary<string, RepositoryFrameworkService> Models { get; set; }
    }
}
