namespace RepositoryFramework
{
    internal class InMemoryGuidableStorage<T> : InMemoryStorage<T, Guid>, IGuidableRepository<T>
    {
        public InMemoryGuidableStorage(RepositoryBehaviorSettings<T, Guid> settings) : base(settings)
        {
        }
    }
}