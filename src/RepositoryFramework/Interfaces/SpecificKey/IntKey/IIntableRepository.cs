namespace RepositoryFramework
{
    public interface IIntableRepository<T> : IRepository<T, int>, IIntableCommand<T>, IIntableQuery<T>, IRepositoryPattern, IQueryPattern, ICommandPattern
    {
    }
}