using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class BoolPopulationService : IRandomPopulationService
    {
        public int Priority => 1;

        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => RandomNumberGenerator.GetInt32(4) > 1;

        public bool IsValid(Type type) 
            => type == typeof(bool) || type == typeof(bool?);
    }
}