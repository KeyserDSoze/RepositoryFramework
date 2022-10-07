namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    internal sealed class DefaultDataverseKeyManager<T, TKey> : IDataverseKeyManager<T, TKey>
        where TKey : notnull
    {
        private readonly Func<T, TKey> _retrievable;
        public DefaultDataverseKeyManager(Func<T, TKey> retrievable)
            => _retrievable = retrievable;

        public string AsString(TKey key)
        {
            if (key is IKey customKey)
                return customKey.AsString();
            return key.ToString()!;
        }
        public TKey Read(T entity)
            => _retrievable.Invoke(entity);
    }
}
