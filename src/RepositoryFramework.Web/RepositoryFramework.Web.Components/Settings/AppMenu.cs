namespace RepositoryFramework.Web.Components
{
    public sealed class AppMenu
    {
        public AppMenu(RepositoryFrameworkRegistry repositoryFrameworkRegistry)
        {
            var navigations = new List<AppMenuSettings>();
            navigations.AddRange(AppInternalSettings.Instance.MenuInternalSettings.Select(x => x.Value));
            foreach (var service in repositoryFrameworkRegistry.Services)
            {
                if (!AppInternalSettings.Instance.NotExposableRepositories.Any(x => x.ToLower() == service.ModelType.Name.ToLower())
                    && !navigations.Any(x => x.ModelType.FullName.ToLower() == service.ModelType.FullName.ToLower()))
                    navigations.Add(AppMenuSettings.CreateDefault(service.ModelType, service.KeyType));
            }
            Navigations = navigations.OrderBy(x => x.Index).ToDictionary(x => x.ModelType.Name.ToLower(), x => x);
        }
        public Dictionary<string, AppMenuSettings> Navigations { get; set; }
    }
}
