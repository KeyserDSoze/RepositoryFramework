namespace RepositoryFramework
{
    public interface IEntity<out T, out TKey> : IEntity
        where TKey : notnull
    {
        TKey Key { get; }
        T Value { get; }
    }
}