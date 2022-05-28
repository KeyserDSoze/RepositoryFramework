namespace RepositoryFramework
{
    public interface IGuidableRepository<T> : IRepository<T, Guid>, IGuidableCommand<T>, IGuidableQuery<T>, IRepositoryPattern, IQueryPattern, ICommandPattern
    {
    }
}