using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace RepositoryFramework.Web.Components
{
    public partial class Settings
    {
        [Inject]
        public AppPaletteWrapper AppPaletteWrapper { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [CascadingParameter(Name = nameof(HttpContext))]
        public HttpContext HttpContext { get; set; }
        private string? _paletteKey;
        protected override async Task OnParametersSetAsync()
        {
            if (!HttpContext.Request.Cookies.TryGetValue(Constant.PaletteKey, out _paletteKey))
                _paletteKey = string.Empty;
            LoadService.Hide();
            await base.OnParametersSetAsync().NoContext();
        }
        private void ChangeSkin(ChangeEventArgs skinName)
        {
            NavigationManager.NavigateTo($"../../../../Repository/Settings/Theme/{skinName.Value}", true);
        }
    }
}
