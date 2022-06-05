namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T> : InMemoryStorage<T, string>, IRepositoryPattern<T>
    {
        public InMemoryStorage(RepositoryBehaviorSettings<T, string, bool> settings) : base(settings)
        {
        }
    }
}