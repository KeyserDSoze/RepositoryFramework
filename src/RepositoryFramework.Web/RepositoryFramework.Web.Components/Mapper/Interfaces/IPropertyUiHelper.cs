using System.Linq.Expressions;

namespace RepositoryFramework.Web
{
    public interface IPropertyUiHelper<T, TKey>
        where TKey : notnull
    {
        Task<Dictionary<string, PropertyUiValue>> ValuesAsync(IServiceProvider serviceProvider);
        IPropertyUiHelper<T, TKey> MapDefault<TProperty>(Expression<Func<T, TProperty>> navigationProperty, TProperty defaultValue);
        IPropertyUiHelper<T, TKey> MapChoice<TProperty>(Expression<Func<T, TProperty>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>> retriever,
            Func<TProperty, string> labelComparer);
        IPropertyUiHelper<T, TKey> MapChoices<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty,
            Func<IServiceProvider, Task<IEnumerable<LabelledPropertyValue>>> retriever,
            Func<TProperty, string> labelComparer);
    }
}
