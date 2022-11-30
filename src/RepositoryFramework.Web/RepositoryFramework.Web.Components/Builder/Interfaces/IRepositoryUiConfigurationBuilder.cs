using System.Linq.Expressions;

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
        IRepositoryUiConfigurationBuilder<TNext, TNextKey> AndConfigure<TNext, TNextKey>()
            where TNextKey : notnull;
    }
}
