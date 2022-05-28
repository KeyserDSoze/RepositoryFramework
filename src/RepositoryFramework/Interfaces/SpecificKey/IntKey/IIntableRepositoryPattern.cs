namespace RepositoryFramework
{
    public interface IIntableRepositoryPattern<T> : IRepository<T, int>, IIntableCommandPattern<T>, IIntableQueryPattern<T>
    {
    }
}