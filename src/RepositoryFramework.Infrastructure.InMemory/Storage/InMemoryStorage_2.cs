namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T, TKey> : InMemoryStorage<T, TKey, State>, IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        public InMemoryStorage(RepositoryBehaviorSettings<T, TKey, State> settings) : base(settings)
        {
        }
    }
}