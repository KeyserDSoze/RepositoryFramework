namespace RepositoryFramework.Client
{
    public interface IStringableRepositoryClient<T> : IStringableRepository<T>, IRepository<T, string>, IStringableCommand<T>, IStringableQuery<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
    }
}