namespace RepositoryFramework
{
    public interface IQuery<T, TKey> : IQueryPattern<T, TKey>, IQueryPattern
        where TKey : notnull
    {

    }
}
