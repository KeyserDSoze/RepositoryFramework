using System.Security.Cryptography;

namespace RepositoryFramework.InMemory.Population
{
    internal class BoolPopulationService : IRandomPopulationService
    {
        public int Priority => 1;

        public dynamic GetValue(RandomPopulationOptions options)
            => RandomNumberGenerator.GetInt32(4) > 1;

        public bool IsValid(Type type) 
            => type == typeof(bool) || type == typeof(bool?);
    }
}