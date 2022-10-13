namespace RepositoryFramework.Infrastructure.MsSql
{
    public interface IMsSqlOptions
    {
        string TableName { get; }
        string ConnectionString { get; }
        string? PrimaryKey { get; }
        string Schema { get; }
        string GetCreationalQueryForTable();
    }
}
