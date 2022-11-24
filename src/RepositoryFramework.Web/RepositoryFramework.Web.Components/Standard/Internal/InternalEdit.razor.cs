using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class InternalEdit<T>
    {
        [Parameter]
        public T? Entity { get; set; }
        [Inject]
        public EntitiesTypeManager EntityManager { get; set; }
        private protected EntityType PropertyTree { get; set; }
        protected override Task OnParametersSetAsync()
        {
            if (Entity == null)
                Entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
            PropertyTree = EntityManager.GetEntity(typeof(T));
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
