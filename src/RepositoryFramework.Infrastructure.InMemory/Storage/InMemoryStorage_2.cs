namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T, TKey> : InMemoryStorage<T, TKey, bool>, IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        public InMemoryStorage(RepositoryBehaviorSettings<T, TKey, bool> settings) : base(settings)
        {
        }
    }
}