namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    public interface IDataverseKeyManager<in T, TKey>
        where TKey : notnull
    {
        TKey Read(T entity);
        string AsString(TKey key);
    }
}
