namespace RepositoryFramework
{
    internal class QueryFacade<T, TKey> : QueryFacade<T, TKey, bool>, IQueryFacade<T, TKey>, IQueryPattern<T, TKey>, IQueryPattern
         where TKey : notnull
    {
        public QueryFacade(IQueryPattern<T, TKey> query) : base(query)
        {
        }
    }
}