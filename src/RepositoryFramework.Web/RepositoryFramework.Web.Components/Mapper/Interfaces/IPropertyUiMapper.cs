namespace RepositoryFramework.Web
{
    public interface IPropertyUiMapper<T, TKey>
        where TKey : notnull
    {
        Task<Dictionary<string, PropertyUiValue>> ValuesAsync(IServiceProvider serviceProvider);
    }
}
