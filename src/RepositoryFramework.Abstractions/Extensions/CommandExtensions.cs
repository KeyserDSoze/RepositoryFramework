namespace RepositoryFramework
{
    public static class CommandExtensions
    {
        public static BatchOperations<T, TKey, TState> CreateBatchOperation<T, TKey, TState>(
            this ICommandPattern<T, TKey, TState> command)
            where TKey : notnull
            where TState : class, IState<T>, new()
        {
            var operations = new BatchOperations<T, TKey, TState>()
            {
                Command = command
            };
            return operations;
        }
    }
}
