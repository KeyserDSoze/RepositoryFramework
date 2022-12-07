using System.Linq.Expressions;

namespace RepositoryFramework.Web
{
    internal sealed class PropertyUiHelper<T, TKey> : IRepositoryPropertyUiHelper<T, TKey>
        where TKey : notnull
    {
        private sealed class RepositoryUiPropertyConfiguratorHelper
        {
            public object? Default { get; set; }
            public Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>>? Retriever { get; set; }
            public bool IsMultiple { get; set; }
            public bool HasTextEditor { get; set; }
            public int MinHeight { get; set; } = 200;
            public Func<object, string>? LabelComparer { get; set; }
        }
        private readonly Dictionary<string, RepositoryUiPropertyConfiguratorHelper> _retrieves = new();
        public async Task<Dictionary<string, PropertyUiSettings>> SettingsAsync(IServiceProvider serviceProvider)
        {
            var values = new Dictionary<string, PropertyUiSettings>();
            foreach (var helper in _retrieves)
            {
                values.Add(helper.Key, new PropertyUiSettings
                {
                    Default = helper.Value.Default,
                    IsMultiple = helper.Value.IsMultiple,
                    HasTextEditor = helper.Value.HasTextEditor,
                    MinHeight = helper.Value.MinHeight,
                    LabelComparer = helper.Value.LabelComparer,
                    Values = helper.Value.Retriever != null ? await helper.Value.Retriever(serviceProvider).NoContext() : null
                });
            }
            return values;
        }
        private RepositoryUiPropertyConfiguratorHelper GetHelper<TProperty>(Expression<Func<T, TProperty>> navigationProperty)
        {
            var name = navigationProperty.Body.ToString();
            name = name.Contains('.') ? name[(name.IndexOf('.') + 1)..] : string.Empty;
            if (!_retrieves.ContainsKey(name))
                _retrieves.Add(name, new RepositoryUiPropertyConfiguratorHelper { });
            var retrieve = _retrieves[name];
            return retrieve;
        }
        public IRepositoryPropertyUiHelper<T, TKey> MapDefault<TProperty>(Expression<Func<T, TProperty>> navigationProperty, TProperty defaultValue)
        {
            var retrieve = GetHelper(navigationProperty);
            retrieve.Default = defaultValue;
            return this;
        }
        public IRepositoryPropertyUiHelper<T, TKey> SetTextEditor<TProperty>(Expression<Func<T, TProperty>> navigationProperty,
            int minHeight)
        {
            var retrieve = GetHelper(navigationProperty);
            retrieve.HasTextEditor = true;
            retrieve.MinHeight = minHeight;
            return this;
        }
        public IRepositoryPropertyUiHelper<T, TKey> MapChoice<TProperty>(Expression<Func<T, TProperty>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>> retriever,
            Func<TProperty, string> labelComparer)
        {
            var retrieve = GetHelper(navigationProperty);
            retrieve.IsMultiple = false;
            retrieve.Retriever = retriever;
            retrieve.LabelComparer = x => x != null ? labelComparer((TProperty)x) : string.Empty;
            return this;
        }
        public IRepositoryPropertyUiHelper<T, TKey> MapChoices<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>> retriever,
            Func<TProperty, string> labelComparer)
        {
            var retrieve = GetHelper(navigationProperty);
            retrieve.IsMultiple = true;
            retrieve.Retriever = retriever;
            retrieve.LabelComparer = x => x != null ? labelComparer((TProperty)x) : string.Empty;
            return this;
        }
    }
}
