using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal sealed class Query<T> : Query<T, string>, IQuery<T>
    {
        public Query(IQueryPattern<T> query) : base(query)
        {
        }
    }
}