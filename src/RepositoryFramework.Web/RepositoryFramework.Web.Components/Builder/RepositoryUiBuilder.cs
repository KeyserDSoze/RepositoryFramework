using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Web.Components
{
    public interface IRepositoryUiConfigurationBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryUiBuilder Builder { get; }
        IRepositoryUiConfigurationBuilder<T, TKey> MapDefault<TProperty>(Expression<Func<T, TProperty>> navigationProperty, TProperty defaultValue);
        IRepositoryUiConfigurationBuilder<T, TKey> MapChoice<TProperty>(Expression<Func<T, TProperty>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<PropertyValue>>> retriever,
            Func<TProperty, string> labelComparer);
        IRepositoryUiConfigurationBuilder<T, TKey> MapChoices<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty,
          Func<IServiceProvider, Task<IEnumerable<PropertyValue>>> retriever,
          Func<TProperty, string> labelComparer);
    }
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
    public sealed class PropertyValue
    {
        public string Label { get; set; }
        public object Value { get; set; }
    }
    public sealed class RepositoryUiPropertyValueRetrieve
    {
        public object? Default { get; set; }
        public Func<IServiceProvider, Task<IEnumerable<PropertyValue>>>? Retriever { get; set; }
        public bool IsMultiple { get; set; }
        public Func<object, string>? LabelComparer { get; set; }
    }
    public sealed class RepositoryUiPropertyValueRetrieved
    {
        public object? Default { get; set; }
        public IEnumerable<PropertyValue>? Values { get; set; }
        public bool IsMultiple { get; set; }
        public Func<object, string>? LabelComparer { get; set; }
    }
    public static class RepositoryUiPropertyValueRetrievedExtensions
    {
        public static bool HasValues(this RepositoryUiPropertyValueRetrieved? retrievd) 
            => retrievd?.Values != null;
    }
    internal sealed class RepositoryUiConfigurationBuilder<T, TKey> : IRepositoryUiConfigurationBuilder<T, TKey>
        where TKey : notnull
    {
        public IRepositoryUiBuilder Builder { get; }
        public RepositoryUiConfigurationBuilder(IRepositoryUiBuilder builder)
            => Builder = builder;
        public IRepositoryUiConfigurationBuilder<T, TKey> MapDefault<TProperty>(Expression<Func<T, TProperty>> navigationProperty, TProperty defaultValue)
        {
            RepositoryUiPropertyValueRetriever<T, TKey>.Instance.MapDefault(navigationProperty, defaultValue);
            return this;
        }

        public IRepositoryUiConfigurationBuilder<T, TKey> MapChoice<TProperty>(Expression<Func<T, TProperty>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<PropertyValue>>> retriever,
            Func<TProperty, string> labelComparer)
        {
            RepositoryUiPropertyValueRetriever<T, TKey>.Instance.MapChoice(navigationProperty, retriever, labelComparer);
            return this;
        }
        public IRepositoryUiConfigurationBuilder<T, TKey> MapChoices<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty,
           Func<IServiceProvider, Task<IEnumerable<PropertyValue>>> retriever,
           Func<TProperty, string> labelComparer)
        {
            RepositoryUiPropertyValueRetriever<T, TKey>.Instance.MapChoices(navigationProperty, retriever, labelComparer);
            return this;
        }
    }
    internal sealed class RepositoryUiBuilder : IRepositoryUiBuilder
    {
        public IServiceCollection Services { get; }
        public RepositoryUiBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IRepositoryUiBuilder WithDefault<T>()
        {
            AppConstant.Instance.RootName = typeof(T).Name;
            Services.AddRazorPages(x =>
            {
                x.Conventions.AddPageRoute($"/Repository/{typeof(T).Name}/Query", "/");
            });
            return this;
        }

        public IRepositoryUiConfigurationBuilder<T, TKey> Configure<T, TKey>()
            where TKey : notnull
            => new RepositoryUiConfigurationBuilder<T, TKey>(this);
    }
}
