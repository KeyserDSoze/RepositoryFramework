namespace RepositoryFramework
{
    internal class InMemoryStringableStorage<T> : InMemoryStorage<T, string>, IStringableRepositoryPattern<T>
    {
        public InMemoryStringableStorage(RepositoryPatternBehaviorSettings<T ,string> settings) : base(settings)
        {
        }
    }
}