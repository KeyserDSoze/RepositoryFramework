using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class InternalEdit<T>
    {
        private List<PropertyInfoKeeper> _primitives;
        private List<PropertyInfoKeeper> _complexes;

        [Parameter]
        public T? Entity { get; set; }
        protected override Task OnParametersSetAsync()
        {
            if (Entity == null)
                Entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
            _primitives = (typeof(T).FetchProperties()
                        .Where(x => x.PropertyType.IsPrimitive())
                        .Select(x => new PropertyInfoKeeper
                        {
                            PropertyInfo = x,
                            Name = x.Name,
                            Label = x.Name,
                        })).ToList();
            _complexes = (typeof(T).FetchProperties()
                        .Where(x => !x.PropertyType.IsPrimitive())
                        .Select(x => new PropertyInfoKeeper
                        {
                            PropertyInfo = x,
                            Name = x.Name,
                            Label = x.Name,
                        })).ToList();
            return base.OnParametersSetAsync();
        }
        private protected RenderFragment LoadNext(PropertyInfoKeeper propertyInfoKeeper)
        {
            var value = propertyInfoKeeper.Value(Entity);
            var genericType = typeof(InternalEdit<>).MakeGenericType(new[] { propertyInfoKeeper.PropertyInfo.PropertyType });
            var frag = new RenderFragment(b =>
            {
                b.OpenComponent(1, genericType);
                b.AddAttribute(2, nameof(Entity), value);
                b.CloseComponent();
            });
            return frag;
        }
    }
}
