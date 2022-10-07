using RepositoryFramework.Infrastructure.Dynamics.Dataverse;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// It populates every object in memory storage injected. You have to use it after service collection build in your startup.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <returns>IServiceProvider</returns>
        public static async ValueTask<IServiceProvider> CreateTableOrMergeNewColumnsInExistingTableAsync(this IServiceProvider serviceProvider)
        {
            foreach (var options in DataverseIntegrations.Instance.Options)
            {
                var client = options.GetClient();
                //to check if a table with that name exists
                //after that check if all the column exists
                //if they don't exist, create all the missing columns
                //change the value of a column
                //for complex column use json
            }
            return serviceProvider;
        }
    }
}
