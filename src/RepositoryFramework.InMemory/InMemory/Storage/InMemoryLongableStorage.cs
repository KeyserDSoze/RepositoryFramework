namespace RepositoryFramework
{
    internal class InMemoryLongableStorage<T> : InMemoryStorage<T, long>, IRepository<T, long>
    {
        public InMemoryLongableStorage(RepositoryBehaviorSettings<T, long> settings) : base(settings)
        {
        }
    }
}