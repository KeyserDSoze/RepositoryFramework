﻿namespace RepositoryFramework.Web.Components
{
    internal sealed class AppMenu : IAppMenu
    {
        internal static List<IRepositoryAppMenuItem> Items { get; } = new List<IRepositoryAppMenuItem>();
        public AppMenu(RepositoryFrameworkRegistry repositoryFrameworkRegistry)
        {
            foreach (var repositoryAppMenuItem in AppInternalSettings.Instance.RepositoryAppMenuItems)
            {
                if (!Navigations.ContainsKey(repositoryAppMenuItem.Key.ToLower()))
                    Navigations.Add(repositoryAppMenuItem.Key.ToLower(), repositoryAppMenuItem.Value);
            }
            foreach (var item in Items)
            {
                if (!Navigations.ContainsKey(item.Name.ToLower()))
                    Navigations.Add(item.Name.ToLower(), item);
            }
            foreach (var service in repositoryFrameworkRegistry.Services)
            {
                if (!AppInternalSettings.Instance.NotExposableRepositories.Any(x => x.ToLower() == service.ModelType.Name.ToLower())
                    && !Navigations.ContainsKey(service.ModelType.Name.ToLower()))
                    Navigations.Add(service.ModelType.Name.ToLower(), RepositoryAppMenuItem.CreateDefault(service.ModelType, service.KeyType));
            }
            Navigations = Navigations.OrderBy(x => x.Value.Index).ToDictionary(x => x.Key, x => x.Value);
        }
        public Dictionary<string, IRepositoryAppMenuItem> Navigations { get; } = new();
    }
}
