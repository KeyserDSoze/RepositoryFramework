using System.Linq.Expressions;

namespace RepositoryFramework.Web.Components
{
    internal sealed class RepositoryUiPropertyValueRetriever<T, TKey>
        where TKey : notnull
    {
        public static RepositoryUiPropertyValueRetriever<T, TKey> Instance { get; } = new();
        private RepositoryUiPropertyValueRetriever() { }
        internal Dictionary<string, RepositoryUiPropertyValueRetrieve> Retrieves { get; } = new();
        internal void MapDefault<TProperty>(Expression<Func<T, TProperty>> navigationProperty, TProperty defaultValue)
        {
            var property = navigationProperty.GetPropertyFromExpression();
            var name = navigationProperty.Body.ToString();
            name = name.Substring(name.IndexOf('.') + 1);
            if (!Retrieves.ContainsKey(name))
                Retrieves.Add(name, new RepositoryUiPropertyValueRetrieve { });
            var retrieve = Retrieves[name];
            retrieve.Default = defaultValue;
        }
        internal void MapChoice<TProperty>(Expression<Func<T, TProperty>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<PropertyValue>>> retriever,
            Func<TProperty, string> labelComparer)
        {
            var property = navigationProperty.GetPropertyFromExpression();
            var name = navigationProperty.Body.ToString();
            name = name.Substring(name.IndexOf('.') + 1);
            if (!Retrieves.ContainsKey(name))
                Retrieves.Add(name, new RepositoryUiPropertyValueRetrieve { });
            var retrieve = Retrieves[name];
            retrieve.IsMultiple = false;
            retrieve.Retriever = retriever;
            retrieve.LabelComparer = x => x != null ? labelComparer((TProperty)x) : string.Empty;
        }
        internal void MapChoices<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<PropertyValue>>> retriever,
            Func<TProperty, string> labelComparer)
        {
            var property = navigationProperty.GetPropertyFromExpression();
            var name = navigationProperty.Body.ToString();
            name = name.Substring(name.IndexOf('.') + 1);
            if (!Retrieves.ContainsKey(name))
                Retrieves.Add(name, new RepositoryUiPropertyValueRetrieve { });
            var retrieve = Retrieves[name];
            retrieve.IsMultiple = true;
            retrieve.Retriever = retriever;
            retrieve.LabelComparer = x => x != null ? labelComparer((TProperty)x) : string.Empty;
        }
    }
}
