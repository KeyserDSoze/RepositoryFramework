using RepositoryFramework.Population;

namespace RepositoryFramework.Services
{
    public interface IInstanceCreator
    {
        object? CreateInstance(RandomPopulationOptions options, object?[]? args = null);
    }
}