namespace RepositoryFramework
{
    internal class Query<T, TKey> : Query<T, TKey, bool>, IQuery<T, TKey>, IQueryPattern<T, TKey>, IQueryPattern
         where TKey : notnull
    {
        public Query(IQueryPattern<T, TKey> query) : base(query)
        {
        }
    }
}