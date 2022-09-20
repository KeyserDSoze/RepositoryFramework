namespace RepositoryFramework
{
    public interface IEntity<out T, out TKey> : IEntity
        where TKey : notnull
    {
        TKey Key { get; }
        T Value { get; }
    }
    public sealed record Entity<T, TKey>(TKey Key, T Value) : IEntity<T, TKey>;
}
