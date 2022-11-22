using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Edit<T, TKey>
        where TKey : notnull
    {
        private static readonly Func<string, TKey> s_keyParser = IKey.Parser<TKey>();
        [Parameter]
        public string Key { get; set; }
        [Parameter]
        public bool EditableKey { get; set; } = true;
        private T? _entity;
        private bool _isNew;
        private TKey _key;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().NoContext();
            if (Queryx != null)
            {
                if (!string.IsNullOrWhiteSpace(Key))
                {
                    _key = s_keyParser(Key);
                    _entity = await Queryx.GetAsync(_key).NoContext();
                }
                if (_entity == null)
                {
                    _entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
                    _isNew = true;
                }
            }
        }
        private void OnPrimitiveChange(string value, PropertyInfoKeeper propertyInfoKeeper)
        {
            propertyInfoKeeper.Set(_entity, value);
        }
        private async Task SaveAsync(bool withRedirect)
        {
            var result = _isNew ?
                await Command.InsertAsync(_key, _entity) :
                await Command.UpdateAsync(_key, _entity);
            if (result.IsOk && withRedirect)
                NavigationManager.NavigateTo($"../../../../Repository/{typeof(T).Name}/Query");
        }
    }
}
