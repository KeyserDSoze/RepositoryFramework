using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class CharPopulationService : ICharPopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => (char)RandomNumberGenerator.GetInt32(256);
    }
}