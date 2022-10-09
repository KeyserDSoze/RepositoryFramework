using Microsoft.PowerPlatform.Dataverse.Client;

namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    public interface IDataverseOptions
    {
        string Environment { get; }
        string Prefix { get; }
        string TableName { get; }
        string TableNameWithPrefix { get; }
        string LogicalTableName { get; }
        string? SolutionName { get; }
        string? Description { get; }
        Type ModelType { get; }
        Type KeyType { get; }
        string PrimaryKey { get; }
        string PrimaryKeyWithPrefix { get; }
        string LogicalPrimaryKey { get; }
        DataverseAppRegistrationAccount? ApplicationIdentity { get; }
        ServiceClient GetClient();
        Task CheckIfExistColumnsAsync();
    }
}
