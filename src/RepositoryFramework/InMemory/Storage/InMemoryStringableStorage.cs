namespace RepositoryFramework
{
    internal class InMemoryStringableStorage<T> : InMemoryStorage<T, string>, IStringableRepository<T>
    {
        public InMemoryStringableStorage(RepositoryPatternBehaviorSettings<T ,string> settings) : base(settings)
        {
        }
    }
}