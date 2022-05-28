namespace RepositoryFramework.Client
{
    public interface IGuidableRepositoryClient<T> : IGuidableRepository<T>, IRepository<T, Guid>, IGuidableCommand<T>, IGuidableQuery<T>, IRepositoryPattern, IQueryPattern, ICommandPattern
    {
    }
}