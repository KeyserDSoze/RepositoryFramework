namespace RepositoryFramework
{
    public interface IEntity<out TKey, out T> where TKey : notnull
    {
        TKey Key { get; }
        T Value { get; }
        public static IEntity<TKey, T> Default(TKey key, T value)
            => new Entity<TKey, T>(key, value);
    }
}