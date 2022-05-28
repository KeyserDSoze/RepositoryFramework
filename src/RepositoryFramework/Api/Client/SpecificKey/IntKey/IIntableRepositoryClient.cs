namespace RepositoryFramework.Client
{
    public interface IIntableRepositoryClient<T> : IIntableRepository<T>, IRepository<T, int>, IIntableCommand<T>, IIntableQuery<T>, IRepositoryPattern, IQueryPattern, ICommandPattern
    {
    }
}