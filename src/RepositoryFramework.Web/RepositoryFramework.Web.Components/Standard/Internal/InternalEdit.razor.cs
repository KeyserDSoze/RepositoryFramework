﻿using System.Collections;
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
        public PropertyBringer PropertyBringer { get; set; }
        private TypeShowcase TypeShowcase { get; set; }
        private IEnumerator<BaseProperty> Enumerator { get; set; }
        protected override Task OnParametersSetAsync()
        {
            if (Entity == null)
                Entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
            TypeShowcase = PropertyBringer.GetEntity(typeof(T));
            Enumerator = TypeShowcase.Properties.GetEnumerator();
            return base.OnParametersSetAsync();
        }
        private protected RenderFragment LoadNext(BaseProperty property)
        {
            if (property.Type == PropertyType.Complex)
            {
                var value = property.Value(Entity);
                var genericType = typeof(InternalEdit<>).MakeGenericType(new[] { property.Self.PropertyType });
                var frag = new RenderFragment(b =>
                {
                    b.OpenComponent(1, genericType);
                    b.AddAttribute(2, nameof(Entity), value);
                    b.CloseComponent();
                });
                return frag;
            }
            else
            {
                var value = property.Value(Entity);
                var genericType = typeof(EnumerableInternalEdit<>).MakeGenericType(property.Generics);
                var frag = new RenderFragment(b =>
                {
                    b.OpenComponent(1, genericType);
                    b.AddAttribute(2, "Entities", value);
                    b.CloseComponent();
                });
                return frag;
            }
        }
    }
}
