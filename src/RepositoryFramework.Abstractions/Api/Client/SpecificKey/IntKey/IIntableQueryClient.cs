namespace RepositoryFramework.Client
{
    public interface IIntableQueryClient<T> : IIntableQuery<T>, IQuery<T, int>, IQueryPattern
    {
    }
}