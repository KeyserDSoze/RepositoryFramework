namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryCreatorBuilder<T, TKey> : RepositoryInMemoryCreatorBuilder<T, TKey, State<T>>, IRepositoryInMemoryCreatorBuilder<T, TKey>
        where TKey : notnull
    {
        public RepositoryInMemoryCreatorBuilder(RepositoryInMemoryBuilder<T, TKey> builder, CreationSettings internalBehaviorSettings) :
            base(builder, internalBehaviorSettings)
        {
        }
    }
}