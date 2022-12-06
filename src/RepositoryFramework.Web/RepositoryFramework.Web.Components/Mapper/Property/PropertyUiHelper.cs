﻿using System.Linq.Expressions;

namespace RepositoryFramework.Web
{
    internal sealed class PropertyUiHelper<T, TKey> : IPropertyUiHelper<T, TKey>
        where TKey : notnull
    {
        private sealed class RepositoryUiPropertyConfiguratorHelper
        {
            public object? Default { get; set; }
            public Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>>? Retriever { get; set; }
            public bool IsMultiple { get; set; }
            public Func<object, string>? LabelComparer { get; set; }
        }
        private readonly Dictionary<string, RepositoryUiPropertyConfiguratorHelper> _retrieves = new();
        public async Task<Dictionary<string, PropertyUiValue>> ValuesAsync(IServiceProvider serviceProvider)
        {
            var values = new Dictionary<string, PropertyUiValue>();
            foreach (var helper in _retrieves)
            {
                values.Add(helper.Key, new PropertyUiValue
                {
                    Default = helper.Value.Default,
                    IsMultiple = helper.Value.IsMultiple,
                    LabelComparer = helper.Value.LabelComparer,
                    Values = helper.Value.Retriever != null ? await helper.Value.Retriever(serviceProvider).NoContext() : null
                });
            }
            return values;
        }
        public IPropertyUiHelper<T, TKey> MapDefault<TProperty>(Expression<Func<T, TProperty>> navigationProperty, TProperty defaultValue)
        {
            var name = navigationProperty.Body.ToString();
            name = name.Substring(name.IndexOf('.') + 1);
            if (!_retrieves.ContainsKey(name))
                _retrieves.Add(name, new RepositoryUiPropertyConfiguratorHelper { });
            var retrieve = _retrieves[name];
            retrieve.Default = defaultValue;
            return this;
        }
        public IPropertyUiHelper<T, TKey> MapChoice<TProperty>(Expression<Func<T, TProperty>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>> retriever,
            Func<TProperty, string> labelComparer)
        {
            var name = navigationProperty.Body.ToString();
            name = name.Substring(name.IndexOf('.') + 1);
            if (!_retrieves.ContainsKey(name))
                _retrieves.Add(name, new RepositoryUiPropertyConfiguratorHelper { });
            var retrieve = _retrieves[name];
            retrieve.IsMultiple = false;
            retrieve.Retriever = retriever;
            retrieve.LabelComparer = x => x != null ? labelComparer((TProperty)x) : string.Empty;
            return this;
        }
        public IPropertyUiHelper<T, TKey> MapChoices<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>> retriever,
            Func<TProperty, string> labelComparer)
        {
            var name = navigationProperty.Body.ToString();
            name = name.Substring(name.IndexOf('.') + 1);
            if (!_retrieves.ContainsKey(name))
                _retrieves.Add(name, new RepositoryUiPropertyConfiguratorHelper { });
            var retrieve = _retrieves[name];
            retrieve.IsMultiple = true;
            retrieve.Retriever = retriever;
            retrieve.LabelComparer = x => x != null ? labelComparer((TProperty)x) : string.Empty;
            return this;
        }
    }
}
