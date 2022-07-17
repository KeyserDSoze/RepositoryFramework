namespace RepositoryFramework
{
    internal class Repository<T, TKey, TState> : IRepository<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState
    {
        private readonly IRepositoryPattern<T, TKey, TState> _repository;

        public Repository(IRepositoryPattern<T, TKey, TState> repository)
        {
            _repository = repository;
        }

        public Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(key, cancellationToken);

        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.GetAsync(key, cancellationToken);

        public Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.InsertAsync(key, value, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => _repository.QueryAsync(options, cancellationToken);
        public Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
           => _repository.CountAsync(options, cancellationToken);

        public Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(key, value, cancellationToken);

        public Task<IEnumerable<BatchResult<TKey, TState>>> BatchAsync(IEnumerable<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default)
            => _repository.BatchAsync(operations, cancellationToken);
    }
}