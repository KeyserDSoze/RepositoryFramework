namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryCreatorBuilder<T, TKey> : IRepositoryInMemoryCreatorBuilder<T, TKey, State<T>>
        where TKey : notnull
    { }
}