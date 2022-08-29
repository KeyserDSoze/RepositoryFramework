namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public class TableStorageOptions<T> : TableStorageOptions<T, string>
    {
        internal static new TableStorageOptions<T> Instance { get; } = new TableStorageOptions<T>();
    }
}
