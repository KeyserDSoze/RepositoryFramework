namespace RepositoryFramework
{
    internal class InMemoryGuidableStorage<T> : InMemoryStorage<T, Guid>, IGuidableRepositoryPattern<T>
    {
        public InMemoryGuidableStorage(RepositoryPatternBehaviorSettings<T, Guid> settings) : base(settings)
        {
        }
    }
}