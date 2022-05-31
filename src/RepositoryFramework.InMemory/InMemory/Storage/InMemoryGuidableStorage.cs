namespace RepositoryFramework
{
    internal class InMemoryGuidableStorage<T> : InMemoryStorage<T, Guid>, IRepository<T, Guid>
    {
        public InMemoryGuidableStorage(RepositoryBehaviorSettings<T, Guid> settings) : base(settings)
        {
        }
    }
}