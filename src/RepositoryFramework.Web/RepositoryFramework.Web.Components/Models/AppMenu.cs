namespace RepositoryFramework.Web.Components
{
    public sealed class AppMenu
    {
        public AppMenu(RepositoryFrameworkRegistry repositoryFrameworkRegistry, IServiceProvider serviceProvider)
        {
            Models = new();
            foreach (var service in repositoryFrameworkRegistry.Services)
            {
                if (serviceProvider.GetService(typeof(RepositoryFrameworkOptions<,>).MakeGenericType(service.ModelType, service.KeyType)) is IRepositoryFrameworkOptions options &&
                    !options.IsNotExposable && !Models.ContainsKey(service.ModelType.Name.ToLower()))
                    Models.Add(service.ModelType.Name.ToLower(), service);
            }
        }
        public Dictionary<string, RepositoryFrameworkService> Models { get; set; }
    }
}
