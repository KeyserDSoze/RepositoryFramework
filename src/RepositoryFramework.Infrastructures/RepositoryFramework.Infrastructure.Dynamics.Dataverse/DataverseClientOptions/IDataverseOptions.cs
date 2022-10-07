using Microsoft.PowerPlatform.Dataverse.Client;

namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    public interface IDataverseOptions
    {
        string Environment { get; }
        string TableName { get; }
        string? SolutionName { get; }
        string? Description { get; }
        Type ModelType { get; }
        Type KeyType { get; }
        DataverseAppRegistrationAccount? ApplicationIdentity { get; }
        ServiceClient GetClient();
    }
}
