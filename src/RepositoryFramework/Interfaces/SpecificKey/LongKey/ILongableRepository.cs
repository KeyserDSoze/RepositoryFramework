namespace RepositoryFramework
{
    public interface ILongableRepository<T> : IRepository<T, long>, ILongableCommand<T>, ILongableQuery<T>, IRepositoryPattern
    {
    }
}