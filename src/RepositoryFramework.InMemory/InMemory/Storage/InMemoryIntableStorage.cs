namespace RepositoryFramework
{
    internal class InMemoryIntableStorage<T> : InMemoryStorage<T, int>, IRepository<T, int>
    {
        public InMemoryIntableStorage(RepositoryBehaviorSettings<T, int> settings) : base(settings)
        {
        }
    }
}