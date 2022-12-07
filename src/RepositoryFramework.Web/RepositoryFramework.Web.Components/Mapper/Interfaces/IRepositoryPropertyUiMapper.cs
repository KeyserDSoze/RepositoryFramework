namespace RepositoryFramework.Web
{
    public interface IRepositoryPropertyUiMapper<T, TKey>
        where TKey : notnull
    {
        Task<Dictionary<string, PropertyUiSettings>> ValuesAsync(IServiceProvider serviceProvider);
    }
}
