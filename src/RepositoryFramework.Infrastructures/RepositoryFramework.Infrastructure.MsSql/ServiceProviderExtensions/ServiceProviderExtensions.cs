﻿using Microsoft.Data.SqlClient;
using RepositoryFramework.Infrastructure.MsSql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// It creates or updates your table in your MsSql. You have to use it after service collection build in your startup.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <returns>IServiceProvider</returns>
        public static async ValueTask<IServiceProvider> MsSqlCreateTableOrMergeNewColumnsInExistingTableAsync(this IServiceProvider serviceProvider)
        {
            foreach (var options in MsSqlIntegrations.Instance.Options)
            {
                if (options.PrimaryKey == null)
                    throw new ArgumentException($"Please install a key in your repository sql for table {options.TableName}");
                using SqlConnection sqlConnection = new(options.ConnectionString);
                sqlConnection.Open();
                var command = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName", sqlConnection);
                command.Parameters.Add(new SqlParameter("TableName", options.TableName));
                var reader = await command.ExecuteReaderAsync();
                List<string> columns = new();
                while (reader != null && await reader.ReadAsync())
                {
                    columns.Add(reader["COLUMN_NAME"].ToString()!);
                }
                await reader!.DisposeAsync();
                if (columns.Count == 0)
                {
                    command = new SqlCommand("SELECT count(*) FROM sys.schemas where name=@Name", sqlConnection);
                    command.Parameters.Add(new SqlParameter("Name", options.Schema));
                    var response = (await command.ExecuteScalarAsync()).Cast<int>();
                    if (response <= 0)
                    {
                        command = new SqlCommand($"CREATE SCHEMA {options.Schema}", sqlConnection);
                        await command.ExecuteNonQueryAsync();
                        command = new SqlCommand("SELECT count(*) FROM sys.schemas where name=@Name", sqlConnection);
                        command.Parameters.Add(new SqlParameter("Name", options.Schema));
                        response = (await command.ExecuteScalarAsync()).Cast<int>();
                        if (response <= 0)
                            throw new ArgumentException($"It was not possible to create a schema {options.Schema} for table {options.TableName}");
                    }
                    command = new SqlCommand(options.GetCreationalQueryForTable(), sqlConnection);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return serviceProvider;
        }
    }
}
