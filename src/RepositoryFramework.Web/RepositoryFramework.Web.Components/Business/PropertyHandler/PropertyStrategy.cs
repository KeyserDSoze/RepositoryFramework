using System.Collections;
using System.Reflection;

namespace RepositoryFramework.Web.Components
{
    internal sealed class PropertyStrategy
    {
        public static PropertyStrategy Instance { get; } = new PropertyStrategy();
        private PropertyStrategy() { }
        public BaseProperty CreateProperty(PropertyInfo propertyInfo, BaseProperty? father)
        {
            if (propertyInfo.PropertyType.IsPrimitive())
                return new PrimitiveProperty(propertyInfo, father);
            else if (propertyInfo.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                return new EnumerableProperty(propertyInfo, father);
            else
                return new ComplexProperty(propertyInfo, father);
        }
    }
}
