namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T> : InMemoryStorage<T, string>, IRepositoryPattern<T>
    {
        public InMemoryStorage(RepositoryBehaviorSettings<T> settings) : base(settings)
        {
        }
    }
}