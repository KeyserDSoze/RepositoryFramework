using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal sealed class QueryFacade<T> : QueryFacade<T, string>, IQueryFacade<T>, IQueryPattern<T>, IQueryPattern
    {
        public QueryFacade(IQueryPattern<T> query) : base(query)
        {
        }
    }
}