namespace RepositoryFramework
{
    public sealed class BatchOperations<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        public List<BatchOperation<T, TKey>> Values { get; init; } = new();
        internal ICommandPattern<T, TKey, TState>? Command { get; init; }
        public BatchOperations<T, TKey, TState> AddInsert(TKey key, T value)
        {
            Values.Add(new BatchOperation<T, TKey>(CommandType.Insert, key, value));
            return this;
        }
        public BatchOperations<T, TKey, TState> AddUpdate(TKey key, T value)
        {
            Values.Add(new BatchOperation<T, TKey>(CommandType.Update, key, value));
            return this;
        }
        public BatchOperations<T, TKey, TState> AddDelete(TKey key)
        {
            Values.Add(new BatchOperation<T, TKey>(CommandType.Delete, key));
            return this;
        }
        public Task<BatchResults<TKey, TState>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (Values.Count > 0 && Command != null)
                return Command.BatchAsync(this, cancellationToken);
            else
                return Task.FromResult(BatchResults<TKey, TState>.Empty);
        }
    }
}
