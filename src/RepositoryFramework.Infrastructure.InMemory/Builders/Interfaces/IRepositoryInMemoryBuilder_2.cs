namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryBuilder<T, TKey>: IRepositoryInMemoryBuilder<T, TKey, State<T>>, IRepositoryBuilder<T, TKey>
        where TKey : notnull
    { }
}