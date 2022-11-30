using System.Linq.Expressions;

namespace RepositoryFramework.Web.Components
{
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
        public IRepositoryUiConfigurationBuilder<TNext, TNextKey> AndConfigure<TNext, TNextKey>()
            where TNextKey : notnull
            => new RepositoryUiConfigurationBuilder<TNext, TNextKey>(Builder);
    }
}
