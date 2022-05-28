namespace RepositoryFramework
{
    internal class InMemoryIntableStorage<T> : InMemoryStorage<T, int>, IIntableRepositoryPattern<T>
    {
        public InMemoryIntableStorage(RepositoryPatternBehaviorSettings<T, int> settings) : base(settings)
        {
        }
    }
}