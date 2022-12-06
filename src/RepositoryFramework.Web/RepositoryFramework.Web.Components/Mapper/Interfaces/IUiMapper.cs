namespace RepositoryFramework.Web
{
    public interface IUiMapper<T, TKey>
        where TKey : notnull
    {
        void Map(IPropertyUiHelper<T, TKey> mapper);
    }
}
