using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class InternalEdit
    {
        [CascadingParameter]
        public EditParametersBearer EditParametersBearer { get; set; }
        [Parameter]
        public BaseProperty BaseProperty { get; set; }
        private string? _containerClass;
        protected override void OnParametersSet()
        {
            _containerClass = BaseProperty.Deep > 3 ? "row row-cols-1" : "row row-cols-1 row-cols-lg-2";
            base.OnParametersSet();
        }
    }
}
