using System.Text.Json;

namespace RepositoryFramework.Services
{
    public interface IRepositoryFrameworkSettings
    {
        JsonSerializerOptions JsonSerializerOptions { get; }
    }
}