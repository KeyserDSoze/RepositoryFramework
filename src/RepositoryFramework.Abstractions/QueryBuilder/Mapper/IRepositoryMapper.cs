namespace RepositoryFramework
{
    public interface IRepositoryMapper<T, TKey, TEntityModel>
        where TKey : notnull
        where T : new()
        where TEntityModel : new()
    {
        T? Map(TEntityModel? entity);
        TEntityModel? Map(T? entity, TKey key);
        TKey? RetrieveKey(TEntityModel? entity);
    }
}
