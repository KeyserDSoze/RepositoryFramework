namespace RepositoryFramework
{
    public static class CommandExtensions
    {
        public static BatchOperations<T, TKey> CreateBatchOperation<T, TKey>(
            this ICommandPattern<T, TKey> command)
            where TKey : notnull
        {
            var operations = new BatchOperations<T, TKey>(command);
            return operations;
        }
    }
}
