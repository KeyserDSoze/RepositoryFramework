using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using RepositoryFramework.Web.Components.Services;

namespace RepositoryFramework.Web.Components
{
    public partial class RepositoryMenu
    {
        [Inject]
        public IAppMenu AppMenu { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public IPolicyEvaluatorManager? PolicyEvaluatorManager { get; set; }
        [CascadingParameter]
        public HttpContext HttpContext { get; set; } = null!;
        public List<AppMenuItem>? _contextAppMenu;
        protected override async Task OnInitializedAsync()
        {
            await VerifyMenuAsync();
            NavigationManager.LocationChanged += (x, location) =>
            {
                _ = VerifyMenuAsync(location.Location);
            };
            await base.OnInitializedAsync().NoContext();
        }

        private async ValueTask VerifyMenuAsync(string? uri = null)
        {
            List<AppMenuItem> contextAppMenu = new();
            var selectedPath = string.Empty;
            if (uri == null)
                selectedPath = $"Repository/{AppInternalSettings.Instance.RootName}/Query";
            else
                selectedPath = new Uri(uri).AbsolutePath;
            foreach (var nav in AppMenu.Navigations.Select(x => x.Value))
            {
                if (nav.Policies.Count == 0 || await PolicyEvaluatorManager.ValidateAsync(HttpContext, nav.Policies))
                {
                    if (nav is IRepositoryAppMenuComplexItem complexItem)
                    {
                        List<AppMenuItem> subItems = new();
                        foreach (var subComplextItem in complexItem.SubMenu)
                        {
                            if (subComplextItem.Policies.Count == 0 || await PolicyEvaluatorManager.ValidateAsync(HttpContext, subComplextItem.Policies))
                            {
                                subItems.Add(new AppMenuItem
                                {
                                    Icon = subComplextItem.Icon,
                                    Name = subComplextItem.Name,
                                    Uri = subComplextItem.Uri,
                                    IsSelected = subComplextItem.Uri.ToLower().Contains(selectedPath.ToLower()),
                                });
                            }
                        }
                        contextAppMenu.Add(new AppMenuItem
                        {
                            Icon = complexItem.Icon,
                            Name = complexItem.Name,
                            IsSelected = subItems.Any(x => x.IsSelected),
                            Items = subItems
                        });
                    }
                    else if (nav is IRepositoryAppMenuSingleItem singleItem)
                    {
                        contextAppMenu.Add(new AppMenuItem
                        {
                            Icon = singleItem.Icon,
                            Name = singleItem.Name,
                            Uri = singleItem.Uri,
                            IsSelected = singleItem.Uri.ToLower().Contains(selectedPath.ToLower()),
                        });
                    }
                }
            }
            _contextAppMenu = contextAppMenu;
            StateHasChanged();
        }
    }
}
