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
        public Dictionary<string, RepositoryUiPropertyValueRetrieved>? PropertiesRetrieved { get; set; }
        [Parameter]
        public string? NavigationPath { get; set; }
        [Parameter]
        public int Deep { get; set; }
        private string _fontSizeForDivider;
        [Inject]
        public required PropertyHandler PropertyHandler { get; set; }
        private TypeShowcase TypeShowcase { get; set; } = null!;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().NoContext();
            Entity ??= typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
            TypeShowcase = PropertyHandler.GetEntity(typeof(T));
            var fontSize = (1.4 - ((float)Deep * 3 / 10));
            if (fontSize < 0.5)
                fontSize = 0.5;
            _fontSizeForDivider = $"font-size: {fontSize.ToString(".0")}em !important";
        }
        private RenderFragment LoadNext(BaseProperty property)
        {
            var value = property.Value(Entity);
            var propertyRetrieved = GetPropertyValueRetrieved(property);
            var nextNavigationPath = GetNextNavigationPath(property);
            if (property.Type == PropertyType.Complex)
            {
                var genericType = typeof(InternalEdit<>).MakeGenericType(new[] { property.Self.PropertyType });
                var frag = new RenderFragment(b =>
                {
                    b.OpenComponent(1, genericType);
                    b.AddAttribute(2, Constant.Entity, value);
                    b.AddAttribute(3, Constant.DisableEdit, DisableEdit);
                    b.AddAttribute(4, Constant.NavigationPath, nextNavigationPath);
                    b.AddAttribute(5, Constant.PropertiesRetrieved, PropertiesRetrieved);
                    b.AddAttribute(6, Constant.Deep, Deep + 1);
                    b.CloseComponent();
                });
                return frag;
            }
            else
            {
                var genericType = typeof(EnumerableInternalEdit<>).MakeGenericType(property.Generics);
                var frag = new RenderFragment(b =>
                {
                    b.OpenComponent(1, genericType);
                    b.AddAttribute(2, Constant.Entities, value);
                    b.AddAttribute(3, Constant.Property, property);
                    b.AddAttribute(4, Constant.Context, Entity);
                    b.AddAttribute(5, Constant.DisableEdit, DisableEdit);
                    b.AddAttribute(6, Constant.NavigationPath, nextNavigationPath);
                    b.AddAttribute(7, Constant.PropertyRetrieved, propertyRetrieved);
                    b.AddAttribute(8, Constant.PropertiesRetrieved, PropertiesRetrieved);
                    b.AddAttribute(9, Constant.Deep, Deep + 1);
                    b.CloseComponent();
                });
                return frag;
            }
        }
        private RenderFragment LoadPrimitiveEdit(BaseProperty property)
        {
            var value = property.Value(Entity);
            var genericType = typeof(InternalPrimitiveEdit<>).MakeGenericType(new[] { property.Self.PropertyType });
            var propertyRetrieved = GetPropertyValueRetrieved(property);
            var frag = new RenderFragment(b =>
           {
               b.OpenComponent(1, genericType);
               b.AddAttribute(2, Constant.Name, property.Self.Name);
               b.AddAttribute(3, Constant.Value, value);
               b.AddAttribute(4, Constant.Update, (object x) => property.Set(Entity, x));
               b.AddAttribute(5, Constant.DisableEdit, DisableEdit);
               b.AddAttribute(6, Constant.PropertyRetrieved, propertyRetrieved);
               b.CloseComponent();
           });
            return frag;
        }
        private string? GetNextNavigationPath(BaseProperty property)
        {
            var navigationPath = NavigationPath;
            if (navigationPath != null)
                navigationPath = $"{NavigationPath}.{property.Self.Name}";
            else
                navigationPath = property.Self.Name;
            return navigationPath;
        }
        private RepositoryUiPropertyValueRetrieved? GetPropertyValueRetrieved(BaseProperty property)
        {
            var nextNavigationPath = GetNextNavigationPath(property);
            var propertyRetrieved = PropertiesRetrieved != null && PropertiesRetrieved.ContainsKey(nextNavigationPath) ? PropertiesRetrieved[nextNavigationPath] : null;
            return propertyRetrieved;
        }
    }
}
