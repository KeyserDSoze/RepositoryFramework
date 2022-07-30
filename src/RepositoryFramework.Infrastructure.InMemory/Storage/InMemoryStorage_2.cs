namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T, TKey> : InMemoryStorage<T, TKey, State<T>>, IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        public InMemoryStorage(RepositoryBehaviorSettings<T, TKey> settings) : base(settings)
        {
        }
    }
}