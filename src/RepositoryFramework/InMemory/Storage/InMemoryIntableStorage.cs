namespace RepositoryFramework
{
    internal class InMemoryIntableStorage<T> : InMemoryStorage<T, int>, IIntableRepository<T>
    {
        public InMemoryIntableStorage(RepositoryPatternBehaviorSettings<T, int> settings) : base(settings)
        {
        }
    }
}