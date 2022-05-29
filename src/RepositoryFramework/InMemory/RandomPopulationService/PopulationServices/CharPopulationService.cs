using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class CharPopulationService : IRandomPopulationService
    {
        public int Priority => 1;

        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
            => (char)RandomNumberGenerator.GetInt32(256);

        public bool IsValid(Type type) 
            => type == typeof(char) || type == typeof(char?);
    }
}