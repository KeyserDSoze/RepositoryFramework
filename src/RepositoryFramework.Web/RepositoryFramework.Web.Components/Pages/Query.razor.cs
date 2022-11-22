using RepositoryFramework.Web.Components.Standard;

namespace RepositoryFramework.Web.Components
{
    public partial class Query
    {
        private protected override Type StandardType => typeof(Query<,>);
        private protected override bool HasKeyInParameters => false;
    }
}
