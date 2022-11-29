using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class InternalEdit<T>
    {
        [Parameter]
        public required T? Entity { get; set; }
        [Parameter]
        public bool DisableEdit { get; set; }
        [Parameter]
        public IEnumerator<string> ColorEnumerator { get; set; }
        [Inject]
        public required PropertyHandler PropertyHandler { get; set; }
        [Inject]
        public AppSettings AppSettings { get; set; }
        private TypeShowcase TypeShowcase { get; set; } = null!;
        private IEnumerator<BaseProperty> Enumerator { get; set; } = null!;
        private string _backgroundColor = string.Empty;
        protected override Task OnParametersSetAsync()
        {
            Entity ??= typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
            if (AppSettings.Palette == AppPalette.Pastels)
            {
                ColorEnumerator ??= Constant.Color.GetPastels().GetEnumerator();
                if (ColorEnumerator.MoveNext())
                    _backgroundColor = ColorEnumerator.Current;
            }
            TypeShowcase = PropertyHandler.GetEntity(typeof(T));
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
                    b.AddAttribute(2, Constant.Entity, value);
                    b.AddAttribute(3, Constant.ColorEnumerator, ColorEnumerator);
                    b.AddAttribute(4, Constant.DisableEdit, DisableEdit);
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
                    b.AddAttribute(2, Constant.Entities, value);
                    b.AddAttribute(3, Constant.Property, property);
                    b.AddAttribute(4, Constant.Context, Entity);
                    b.AddAttribute(5, Constant.DisableEdit, DisableEdit);
                    b.AddAttribute(6, Constant.ColorEnumerator, ColorEnumerator);
                    b.CloseComponent();
                });
                return frag;
            }
        }
    }
}
