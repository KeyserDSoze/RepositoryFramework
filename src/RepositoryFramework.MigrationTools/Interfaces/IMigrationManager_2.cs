namespace RepositoryFramework.Migration
{
    public interface IMigrationManager<T, TKey> : IMigrationManager<T, TKey, State>
        where TKey : notnull
    {
    }
}