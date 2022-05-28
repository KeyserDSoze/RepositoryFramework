namespace RepositoryFramework
{
    public interface ILongableRepositoryPattern<T> : IRepository<T, long>, ILongableCommandPattern<T>, ILongableQueryPattern<T>
    {
    }
}