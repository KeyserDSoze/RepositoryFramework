using System.Reflection;

namespace RepositoryFramework.Web.Components
{
    public sealed class PrimitiveProperty : BaseProperty
    {
        public PrimitiveProperty(PropertyInfo info, BaseProperty? father) : base(info, father)
        {
            Type = PropertyType.Primitive;
        }

        public override IEnumerable<BaseProperty> GetQueryableProperty()
        {
            yield return this;
        }

        protected override void ConstructWell()
        {

        }
    }
}
