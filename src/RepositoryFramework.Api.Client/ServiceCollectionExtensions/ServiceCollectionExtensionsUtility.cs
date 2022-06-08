using RepositoryFramework.ApiClient;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private enum ClientType
        {
            Repository,
            Query,
            Command
        }
    }
}