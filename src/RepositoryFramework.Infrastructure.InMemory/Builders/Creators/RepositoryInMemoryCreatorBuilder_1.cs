namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryCreatorBuilder<T> : RepositoryInMemoryCreatorBuilder<T, string>, IRepositoryInMemoryCreatorBuilder<T>
    {
        public RepositoryInMemoryCreatorBuilder(RepositoryInMemoryBuilder<T> builder, CreationSettings internalBehaviorSettings) :
            base(builder, internalBehaviorSettings)
        {
        }
    }
}