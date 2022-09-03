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

        public IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(Query query, CancellationToken cancellationToken = default)
            => _repository.QueryAsync(query.Translate<T>(), cancellationToken);
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            Query query,
            CancellationToken cancellationToken = default)
           => _repository.OperationAsync(operation, query.Translate<T>(), cancellationToken);

        public Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(key, value, cancellationToken);

        public Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
            => _repository.BatchAsync(operations, cancellationToken);
    }
}