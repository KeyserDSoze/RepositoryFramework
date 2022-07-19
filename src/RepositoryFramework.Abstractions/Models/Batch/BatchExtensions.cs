namespace RepositoryFramework
{
    public static class BatchExtensions
    {
        public static Task ExecuteAsync<T, TKey, TState>(
            this List<BatchOperation<T, TKey>> operations,
            ICommandPattern<T, TKey, TState> command,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            where TState : class, IState<T>, new()
            => command.BatchAsync(operations, cancellationToken);
        public static List<BatchOperation<T, TKey>> AddInsert<T, TKey>(
            this List<BatchOperation<T, TKey>> operations,
            TKey key,
            T value)
            where TKey : notnull
        {
            operations.Add(new BatchOperation<T, TKey>(CommandType.Insert, key, value));
            return operations;
        }
        public static List<BatchOperation<T, TKey>> AddUpdate<T, TKey>(
            this List<BatchOperation<T, TKey>> operations,
            TKey key,
            T value)
            where TKey : notnull
        {
            operations.Add(new BatchOperation<T, TKey>(CommandType.Update, key, value));
            return operations;
        }
        public static List<BatchOperation<T, TKey>> AddDelete<T, TKey>(
            this List<BatchOperation<T, TKey>> operations,
            TKey key)
            where TKey : notnull
        {
            operations.Add(new BatchOperation<T, TKey>(CommandType.Delete, key));
            return operations;
        }
    }
}
