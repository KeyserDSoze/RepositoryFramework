namespace RepositoryFramework
{
    internal class InMemoryLongableStorage<T> : InMemoryStorage<T, long>, ILongableRepositoryPattern<T>
    {
        public InMemoryLongableStorage(RepositoryPatternBehaviorSettings<T, long> settings) : base(settings)
        {
        }
    }
}