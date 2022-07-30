namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryBuilder<T> : IRepositoryInMemoryBuilder<T, string>, IRepositoryBuilder<T>
    { }
}