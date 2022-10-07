using Microsoft.PowerPlatform.Dataverse.Client;

namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    public sealed class DataverseOptions<T, TKey> : IDataverseOptions
    {
        public string Environment { get; set; } = null!;
        public string TableName { get; set; } = typeof(T).Name;
        public string? SolutionName { get; set; }
        public string? Description { get; set; }
        public DataverseAppRegistrationAccount? ApplicationIdentity { get; set; } = null!;
        public Type ModelType { get; } = typeof(T);
        public Type KeyType { get; } = typeof(TKey);
        public ServiceClient GetClient() => new(@$"Url=https://{Environment}.dynamics.com;AuthType=ClientSecret;ClientId={ApplicationIdentity!.ClientId};ClientSecret={ApplicationIdentity!.ClientSecret};RequireNewInstance=true");
    }
}
