namespace RepositoryFramework.Population
{
    public record RandomPopulationOptions(Type Type, 
        IPopulationService PopulationService, int NumberOfEntities, 
        string TreeName);
}