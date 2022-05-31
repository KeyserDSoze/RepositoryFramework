using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class CharPopulationService : IRandomPopulationService
    {
        public int Priority => 1;

        public dynamic GetValue(RandomPopulationOptions options)
            => (char)RandomNumberGenerator.GetInt32(256);

        public bool IsValid(Type type) 
            => type == typeof(char) || type == typeof(char?);
    }
}