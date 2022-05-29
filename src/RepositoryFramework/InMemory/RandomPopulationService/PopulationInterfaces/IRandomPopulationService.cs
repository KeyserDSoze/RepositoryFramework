namespace RepositoryFramework.Population
{
    public interface IRandomPopulationService
    {
        int Priority { get; }
        bool IsValid(Type type);
        dynamic GetValue(RandomPopulationOptions options);
    }
}