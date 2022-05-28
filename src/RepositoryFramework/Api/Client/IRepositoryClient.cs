namespace RepositoryFramework.Client
{
    public interface IRepositoryClient<T, TKey> : IRepository<T, TKey>, ICommand<T, TKey>, IQuery<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }
}