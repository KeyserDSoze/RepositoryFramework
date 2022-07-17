namespace RepositoryFramework
{
    public sealed record BatchWrapper<T, TKey, TState>
        (ICommandPattern<T, TKey, TState> Command, List<BatchOperation<T, TKey>> Operations)
        where TKey : notnull
        where TState : class, IState
    {
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (Operations.Count > 0)
                return Command.BatchAsync(Operations, cancellationToken);
            else
                return Task.CompletedTask;
        }

        public BatchWrapper<T, TKey, TState> AddInsert(TKey key, T value)
        {
            Operations.AddInsert(key, value);
            return this;
        }
        public BatchWrapper<T, TKey, TState> AddUpdate(TKey key, T value)
        {
            Operations.AddUpdate(key, value);
            return this;
        }
        public BatchWrapper<T, TKey, TState> AddDelete(TKey key)
        {
            Operations.AddDelete(key);
            return this;
        }
    }
}
