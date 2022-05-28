namespace RepositoryFramework
{
    public interface IStringableRepositoryPattern<T> : IRepository<T, string>, IStringableCommandPattern<T>, IStringableQueryPattern<T>
    {
    }
}