namespace RepositoryFramework
{
    public interface IStringableQuery<T> : IQuery<T, string>, IQueryPattern
    {
    }
}