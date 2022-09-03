namespace RepositoryFramework
{
    public interface IEntity
    {
        public static IEntity<T, TKey> Default<T, TKey>(TKey key, T value)
            where TKey : notnull
            => new Entity<T, TKey>(key, value);
    }
    public interface IEntity<out T, out TKey> : IEntity
        where TKey : notnull
    {
        TKey Key { get; }
        T Value { get; }
    }
}