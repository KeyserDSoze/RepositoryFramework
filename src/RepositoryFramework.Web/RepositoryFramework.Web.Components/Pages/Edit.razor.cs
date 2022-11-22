using Microsoft.AspNetCore.Components;
using RepositoryFramework.Web.Components.Standard;

namespace RepositoryFramework.Web.Components
{
    public partial class Edit
    {
        private protected override Type StandardType => typeof(Edit<,>);
        private protected override bool HasKeyInParameters => true;
    }
}
