using System.Reflection;
using System;
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
        public Dictionary<string, PropertyUiSettings>? PropertiesUiSettings { get; set; }
        [Parameter]
        public string NavigationPath { get; set; } = string.Empty;
        [Parameter]
        public int Deep { get; set; }
        private string? _fontSizeForDivider;
        [Inject]
        public required PropertyHandler PropertyHandler { get; set; }
        private TypeShowcase TypeShowcase { get; set; } = null!;
        private PropertyUiSettings? _entitySettings;
        private readonly Dictionary<string, object> _restorableValues = new();
        private T? _restorableValue;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().NoContext();
            if (PropertiesUiSettings != null && PropertiesUiSettings.ContainsKey(NavigationPath))
                _entitySettings = PropertiesUiSettings[NavigationPath];
            if (Entity == null || Entity.Equals(default(T)))
            {
                if (_entitySettings?.Default != null)
                    Entity = (T)_entitySettings.Default;
                else
                    Entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
            }
            TypeShowcase = PropertyHandler.GetEntity(typeof(T));
            var fontSize = (1.4 - ((float)Deep * 3 / 10));
            if (fontSize < 0.5)
                fontSize = 0.5;
            _fontSizeForDivider = $"font-size: {fontSize:.0}em !important";
        }
        private RenderFragment LoadNext(BaseProperty property)
        {
            var value = property.Value(Entity);
            var propertyUiSettings = GetPropertySettings(property);
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
                    b.AddAttribute(5, Constant.PropertiesUiSettings, PropertiesUiSettings);
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
                    b.AddAttribute(7, Constant.PropertyUiSettings, propertyUiSettings);
                    b.AddAttribute(8, Constant.PropertiesUiSettings, PropertiesUiSettings);
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
            var propertyUiSettings = GetPropertySettings(property);
            var frag = new RenderFragment(b =>
           {
               b.OpenComponent(1, genericType);
               b.AddAttribute(2, Constant.Name, property.Self.Name);
               b.AddAttribute(3, Constant.Value, value);
               b.AddAttribute(4, Constant.Update, (object x) => property.Set(Entity, x));
               b.AddAttribute(5, Constant.DisableEdit, DisableEdit);
               b.AddAttribute(6, Constant.PropertyUiSettings, propertyUiSettings);
               b.CloseComponent();
           });
            return frag;
        }
        private string? GetNextNavigationPath(BaseProperty property)
        {
            var navigationPath = NavigationPath;
            if (!string.IsNullOrWhiteSpace(navigationPath))
                navigationPath = $"{NavigationPath}.{property.Self.Name}";
            else
                navigationPath = property.Self.Name;
            return navigationPath;
        }
        private PropertyUiSettings? GetPropertySettings(BaseProperty property)
        {
            var nextNavigationPath = GetNextNavigationPath(property);
            var propertyUiSettings = PropertiesUiSettings != null && PropertiesUiSettings.ContainsKey(nextNavigationPath) ? PropertiesUiSettings[nextNavigationPath] : null;
            return propertyUiSettings;
        }
        public void SetDefault(BaseProperty property, object value)
        {
            if (!_restorableValues.ContainsKey(property.NavigationPath))
                _restorableValues.Add(property.NavigationPath, property.Value(Entity));
            property.Set(Entity, value.ToDeepCopy());
        }
        public void Restore(BaseProperty property)
        {
            if (_restorableValues.ContainsKey(property.NavigationPath))
            {
                _restorableValues.Remove(property.NavigationPath, out var value);
                property.Set(Entity, value);
            }
        }
        public void SetDefault()
        {
            if (_entitySettings != null)
            {
                _restorableValue = Entity.ToDeepCopy();
                Entity!.CopyPropertiesFrom(_entitySettings.Default.ToDeepCopy());
            }
        }
        public void Restore()
        {
            Entity.CopyPropertiesFrom(_restorableValue);
            _restorableValue = default;
        }
    }
}
