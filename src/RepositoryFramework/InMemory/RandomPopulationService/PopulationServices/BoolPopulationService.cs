using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class BoolPopulationService<T, TKey> : IBoolPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
            => RandomNumberGenerator.GetInt32(4) > 1;
    }
}