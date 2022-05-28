namespace RepositoryFramework
{
    public interface IGuidableRepositoryPattern<T> : IRepository<T, Guid>, IGuidableCommandPattern<T>, IGuidableQueryPattern<T>
    {
    }
}