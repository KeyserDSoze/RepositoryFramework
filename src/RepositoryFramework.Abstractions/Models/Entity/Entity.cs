namespace RepositoryFramework
{
    internal class Entity<T, TKey> : IEntity<T, TKey>
        where TKey : notnull
    {
        public TKey Key { get; }
        public T Value { get; }
        public Entity(TKey key, T value)
        {
            Key = key;
            Value = value;
        }
    }
}