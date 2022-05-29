namespace RepositoryFramework
{
    public interface IGuidableQuery<T> : IQuery<T, Guid>, IQueryPattern
    {
    }
}