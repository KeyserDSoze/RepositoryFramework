using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal sealed class Query<T> : Query<T, string>, IQuery<T>, IQueryPattern<T>, IQueryPattern
    {
        public Query(IQueryPattern<T> query) : base(query)
        {
        }
    }
}