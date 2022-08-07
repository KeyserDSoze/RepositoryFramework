using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class Repository<T, TKey> : IRepository<T, TKey> 
        where TKey : notnull
    {
        private readonly IRepositoryPattern<T, TKey> _repository;

        public Repository(IRepositoryPattern<T, TKey> repository)
        {
            _repository = repository;
        }

        public Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(key, cancellationToken);

        public Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.GetAsync(key, cancellationToken);

        public Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.InsertAsync(key, value, cancellationToken);

        public IAsyncEnumerable<T> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => _repository.QueryAsync(options, cancellationToken);
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            QueryOptions<T>? options = null,
            CancellationToken cancellationToken = default)
           => _repository.OperationAsync(operation, options, cancellationToken);

        public Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(key, value, cancellationToken);

        public Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
            => _repository.BatchAsync(operations, cancellationToken);
    }
}