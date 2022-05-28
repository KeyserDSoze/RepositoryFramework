namespace RepositoryFramework
{
    public interface IStringableRepository<T> : IRepository<T, string>, IStringableCommand<T>, IStringableQuery<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
    }
}