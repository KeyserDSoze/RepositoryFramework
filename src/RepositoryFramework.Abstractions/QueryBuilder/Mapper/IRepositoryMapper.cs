namespace RepositoryFramework
{
    public interface IRepositoryMapper<T, TKey, TEntityModel>
        where TKey : notnull
    {
        T? Map(TEntityModel? entity);
        TEntityModel? Map(T? entity, TKey key);
        TKey? RetrieveKey(TEntityModel? entity);
    }
}
