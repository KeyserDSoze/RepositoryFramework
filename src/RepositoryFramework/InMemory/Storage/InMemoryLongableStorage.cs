namespace RepositoryFramework
{
    internal class InMemoryLongableStorage<T> : InMemoryStorage<T, long>, ILongableRepository<T>
    {
        public InMemoryLongableStorage(RepositoryBehaviorSettings<T, long> settings) : base(settings)
        {
        }
    }
}