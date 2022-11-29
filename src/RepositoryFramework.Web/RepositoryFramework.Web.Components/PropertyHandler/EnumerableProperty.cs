using System.Linq.Dynamic.Core;
using System.Reflection;

namespace RepositoryFramework.Web.Components
{
    public sealed class EnumerableProperty : BaseProperty
    {
        public EnumerableProperty(PropertyInfo info, BaseProperty? father) : base(info, father)
        {
            Type = PropertyType.Enumerable;
        }

        public override IEnumerable<BaseProperty> GetQueryableProperty()
        {
            yield return this;
        }

        protected override void ConstructWell()
        {
            var enumerable = Self.PropertyType.GetInterfaces().FirstOrDefault(x => x.Name.StartsWith("IEnumerable`1"));
            Generics = enumerable?.GetGenericArguments();
            if (Generics != null)
                foreach (var generic in Generics)
                {
                    if (!generic.IsPrimitive())
                        foreach (var property in generic.FetchProperties())
                            Sons.Add(PropertyStrategy.Instance.CreateProperty(property, this));
                }
        }
    }
}
