namespace RepositoryFramework
{
    internal class InMemoryStringableStorage<T> : InMemoryStorage<T, string>, IRepository<T, string>
    {
        public InMemoryStringableStorage(RepositoryBehaviorSettings<T ,string> settings) : base(settings)
        {
        }
    }
}