namespace RepositoryFramework
{
    internal class Query<T, TKey> : Query<T, TKey, State>, IQuery<T, TKey>
        where TKey : notnull
    {
        public Query(IQueryPattern<T, TKey> query) : base(query)
        {
        }
    }
}