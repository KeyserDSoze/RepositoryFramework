namespace RepositoryFramework
{
    internal class InMemoryStringableStorage<T> : InMemoryStorage<T, string>, IStringableRepository<T>
    {
        public InMemoryStringableStorage(RepositoryBehaviorSettings<T ,string> settings) : base(settings)
        {
        }
    }
}