using System.Text.Json;

namespace RepositoryFramework.Services
{
    internal class RepositoryFrameworkSettings : IRepositoryFrameworkSettings
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions();
    }
}
