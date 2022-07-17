namespace RepositoryFramework
{
    public static class CommandExtensions
    {
        public static BatchWrapper<T, TKey, TState> CreateBatchOperation<T, TKey, TState>(
            this ICommandPattern<T, TKey, TState> command)
            where TKey : notnull
            where TState : class, IState
            => new(command, new());
    }
}
