using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class Query<T, TKey> : IQuery<T, TKey>, IQueryPattern<T, TKey>, IQueryPattern
         where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey> _repository;

        public Query(IQueryPattern<T, TKey> repository)
        {
            _repository = repository;
        }

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.GetAsync(key, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
            => _repository.QueryAsync(predicate, top, skip, cancellationToken);
    }
}