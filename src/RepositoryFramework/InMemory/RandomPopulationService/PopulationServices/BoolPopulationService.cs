using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class BoolPopulationService : IBoolPopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => RandomNumberGenerator.GetInt32(4) > 1;
    }
}