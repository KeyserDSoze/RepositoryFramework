using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components
{
    public partial class RepositoryMenu
    {
        [Inject]
        public AppMenu AppMenu { get; set; }

        [Inject]
        public AppSettings Settings { get; set; }

        [Parameter]
        public RenderFragment? EndFragment { get; set; }

        [Parameter]
        public RenderFragment? StartFragment { get; set; }
    }
}
