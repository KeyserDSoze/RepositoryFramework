namespace RepositoryFramework
{
    public sealed class BatchOperations<T, TKey>
        where TKey : notnull
    {
        public List<BatchOperation<T, TKey>> Values { get; init; } = new();
        private readonly ICommandPattern<T, TKey>? _command;
        internal BatchOperations(ICommandPattern<T, TKey>? command)
        {
            _command = command;
        }
        public BatchOperations<T, TKey> AddInsert(TKey key, T value)
        {
            Values.Add(new BatchOperation<T, TKey>(CommandType.Insert, key, value));
            return this;
        }
        public BatchOperations<T, TKey> AddUpdate(TKey key, T value)
        {
            Values.Add(new BatchOperation<T, TKey>(CommandType.Update, key, value));
            return this;
        }
        public BatchOperations<T, TKey> AddDelete(TKey key)
        {
            Values.Add(new BatchOperation<T, TKey>(CommandType.Delete, key));
            return this;
        }
#warning Alessandro Rapiti - check if I want to read this in method, I don't like the two ways. In BatchAsync and here.
        public Task<BatchResults<T, TKey>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (Values.Count > 0 && _command != null)
                return _command.BatchAsync(this, cancellationToken);
            else
                return Task.FromResult(BatchResults<T, TKey>.Empty);
        }
    }
}
