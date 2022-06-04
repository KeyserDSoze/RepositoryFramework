namespace RepositoryFramework.InMemory.Population
{
    /// <summary>
    /// Interface that helps the creation of a new instance of object during random population.
    /// </summary>
    public interface IInstanceCreator
    {
        object? CreateInstance(RandomPopulationOptions options, object?[]? args = null);
    }
}