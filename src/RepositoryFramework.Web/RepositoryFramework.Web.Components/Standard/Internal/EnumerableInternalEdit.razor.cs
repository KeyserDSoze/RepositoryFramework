using System.Collections;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class EnumerableInternalEdit
    {
        [CascadingParameter]
        public EditParametersBearer EditParametersBearer { get; set; }
        [Parameter]
        public BaseProperty BaseProperty { get; set; } = null!;

        private IEnumerable _entities;
        private string? _error;
        private PropertyUiSettings? _propertyUiSettings;
        protected override void OnParametersSet()
        {
            var retrieveEntities = EditParametersBearer.GetValue(BaseProperty);
            if (retrieveEntities.Exception == null)
            {
                _entities = retrieveEntities.Entity as IEnumerable;
                _propertyUiSettings = EditParametersBearer.GetSettings(BaseProperty);
            }
            else
                _error = retrieveEntities.Exception.Message;
            base.OnParametersSet();
        }
        private void New()
        {
            Type[] types = BaseProperty.Self.PropertyType.IsArray ? new Type[1] { BaseProperty.Self.PropertyType.GetElementType() } :
                BaseProperty.Self.PropertyType.GetGenericArguments();
            var entity = types.First().CreateWithDefaultConstructorPropertiesAndField()!;
            if (_entities is IList list)
                list.Add(entity);
        }
    }
}
