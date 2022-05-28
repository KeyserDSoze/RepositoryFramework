namespace RepositoryFramework
{
    public interface IRepositoryPattern : ICommandPattern, IQueryPattern { }
    public interface IRepository<T, TKey> : ICommand<T, TKey>, IQuery<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }
}