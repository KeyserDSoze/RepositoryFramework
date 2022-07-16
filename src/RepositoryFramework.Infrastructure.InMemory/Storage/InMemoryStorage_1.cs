namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T> : InMemoryStorage<T, string>, IRepositoryPattern<T>
    {
        public InMemoryStorage(RepositoryBehaviorSettings<T, string, State> settings) : base(settings)
        {
        }
    }
}