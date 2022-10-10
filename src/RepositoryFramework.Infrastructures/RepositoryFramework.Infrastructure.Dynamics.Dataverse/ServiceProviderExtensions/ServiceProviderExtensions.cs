﻿using System.Reflection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
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
        public static async ValueTask<IServiceProvider> DataverseCreateTableOrMergeNewColumnsInExistingTableAsync(this IServiceProvider serviceProvider)
        {
            foreach (var options in DataverseIntegrations.Instance.Options)
            {
                var serviceClient = options.GetClient();
                RetrieveEntityRequest retrieveEntityRequest = new()
                {
                    LogicalName = options.LogicalTableName,
                    EntityFilters = EntityFilters.All
                };
                var response = await Try.WithDefaultOnCatchAsync(
                    () => serviceClient.ExecuteAsync(retrieveEntityRequest));
                if (response.Entity == default)
                {
                    CreateEntityRequest createrequest = new()
                    {
                        SolutionUniqueName = options.SolutionName,
                        Entity = new EntityMetadata
                        {
                            SchemaName = options.TableNameWithPrefix,
                            DisplayName = new Label(options.TableName, 1033),
                            DisplayCollectionName = new Label(options.TableName, 1033),
                            Description = new Label(options.Description ?? $"A table to store information about {options.TableName} entity.", 1033),
                            OwnershipType = OwnershipTypes.UserOwned,
                            IsActivity = false,

                        },
                        PrimaryAttribute = new StringAttributeMetadata
                        {
                            SchemaName = options.PrimaryKeyWithPrefix,
                            RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                            MaxLength = 100,
                            FormatName = StringFormatName.Text,
                            DisplayName = new Label(options.PrimaryKey, 1033),
                            Description = new Label($"The primary attribute for the {options.TableName} entity.", 1033)
                        }

                    };
                    var creationResponse = await Try.WithDefaultOnCatchAsync(
                        () => serviceClient.ExecuteAsync(createrequest));
                    if (creationResponse.Entity == default)
                        throw new ArgumentException($"Error in table creation for {options.TableName}");
                }
                await options.CheckIfExistColumnsAsync();
            }
            return serviceProvider;
        }
    }
}
