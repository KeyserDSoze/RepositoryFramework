namespace RepositoryFramework
{
    internal class Query<T, TKey> : Query<T, TKey, State<T>>, IQuery<T, TKey>
        where TKey : notnull
    {
        public Query(IQueryPattern<T, TKey> query) : base(query)
        {
        }
    }
}