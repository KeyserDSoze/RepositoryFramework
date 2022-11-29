using System.Reflection;

namespace RepositoryFramework.Web.Components
{
    public sealed class TypeShowcase
    {
        public List<BaseProperty> Properties { get; } = new();
        public TypeShowcase(Type type)
        {
            foreach (var property in type.FetchProperties())
                Properties.Add(PropertyStrategy.Instance.CreateProperty(property, null));
        }
        public IEnumerable<BaseProperty> GetAllPrimiviteProperties()
        {
            foreach (var property in Properties)
                foreach (var x in property.GetQueryableProperty())
                    yield return x;
        }
    }
}
