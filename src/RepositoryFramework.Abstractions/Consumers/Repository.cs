namespace RepositoryFramework
{
    internal class Repository<T, TKey> : IRepository<T, TKey>
        where TKey : notnull
    {
        private readonly IRepositoryPattern<T, TKey> _repository;
        private readonly IRepositoryBusinessManager<T, TKey>? _businessManager;

        public Repository(IRepositoryPattern<T, TKey> repository, IRepositoryBusinessManager<T, TKey>? businessManager = null)
        {
            _repository = repository;
            _businessManager = businessManager;
        }

        public Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeDelete == true || _businessManager?.HasBusinessAfterDelete == true ?
                _businessManager.DeleteAsync(_repository, key, cancellationToken) : _repository.DeleteAsync(key, cancellationToken);
        public Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeExist == true || _businessManager?.HasBusinessAfterExist == true ?
                _businessManager.ExistAsync(_repository, key, cancellationToken) : _repository.ExistAsync(key, cancellationToken);
        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeGet == true || _businessManager?.HasBusinessAfterGet == true ?
                _businessManager.GetAsync(_repository, key, cancellationToken) : _repository.GetAsync(key, cancellationToken);
        public Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeInsert == true || _businessManager?.HasBusinessAfterInsert == true ?
                _businessManager.InsertAsync(_repository, key, value, cancellationToken) : _repository.InsertAsync(key, value, cancellationToken);
        public IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(Query query, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeQuery == true || _businessManager?.HasBusinessAfterQuery == true ?
                   _businessManager.QueryAsync(_repository, query.Translate<T>(), cancellationToken) : _repository.QueryAsync(query.Translate<T>(), cancellationToken);
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            Query query,
            CancellationToken cancellationToken = default)
           => _businessManager?.HasBusinessBeforeOperation == true || _businessManager?.HasBusinessAfterOperation == true ?
                _businessManager.OperationAsync(_repository, operation, query, cancellationToken) : _repository.OperationAsync(operation, query, cancellationToken);
        public Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeUpdate == true || _businessManager?.HasBusinessAfterUpdate == true ?
                _businessManager.UpdateAsync(_repository, key, value, cancellationToken) : _repository.UpdateAsync(key, value, cancellationToken);
        public Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeBatch == true || _businessManager?.HasBusinessAfterBatch == true ?
                _businessManager.BatchAsync(_repository, operations, cancellationToken) : _repository.BatchAsync(operations, cancellationToken);
    }
}
