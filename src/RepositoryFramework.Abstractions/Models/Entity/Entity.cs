namespace RepositoryFramework
{
    internal class Entity<TKey, T> : IEntity<TKey, T>
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