namespace RepositoryFramework
{
    internal class InMemoryGuidableStorage<T> : InMemoryStorage<T, Guid>, IGuidableRepository<T>
    {
        public InMemoryGuidableStorage(RepositoryPatternBehaviorSettings<T, Guid> settings) : base(settings)
        {
        }
    }
}