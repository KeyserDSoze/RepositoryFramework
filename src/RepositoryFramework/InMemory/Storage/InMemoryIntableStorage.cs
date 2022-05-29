namespace RepositoryFramework
{
    internal class InMemoryIntableStorage<T> : InMemoryStorage<T, int>, IIntableRepository<T>
    {
        public InMemoryIntableStorage(RepositoryBehaviorSettings<T, int> settings) : base(settings)
        {
        }
    }
}