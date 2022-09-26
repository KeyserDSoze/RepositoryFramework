namespace RepositoryFramework
{
    internal class Repository<T, TKey> : IRepository<T, TKey>
        where TKey : notnull
    {
        private readonly Lazy<Query<T, TKey>> _query;
        private readonly Lazy<Command<T, TKey>> _command;

        public Repository(IRepositoryPattern<T, TKey> repository,
            IRepositoryBusinessManager<T, TKey>? businessManager = null)
        {
            _query = new Lazy<Query<T, TKey>>(() => new Query<T, TKey>(repository, businessManager));
            _command = new Lazy<Command<T, TKey>>(() => new Command<T, TKey>(repository, businessManager));
        }

        public Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.Value.ExistAsync(key, cancellationToken);
        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.Value.GetAsync(key, cancellationToken);
        public IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _query.Value.QueryAsync(filter, cancellationToken);
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            IFilterExpression filter,
            CancellationToken cancellationToken = default)
           => _query.Value.OperationAsync(operation, filter, cancellationToken);

        public Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.Value.InsertAsync(key, value, cancellationToken);
        public Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.Value.UpdateAsync(key, value, cancellationToken);
        public Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
           => _command.Value.DeleteAsync(key, cancellationToken);
        public Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
            => _command.Value.BatchAsync(operations, cancellationToken);
    }
}
