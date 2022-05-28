using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class BytePopulationService<T, TKey> : IBytePopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {
            if (type == typeof(byte) || type == typeof(byte?))
                return RandomNumberGenerator.GetBytes(1)[0];
            else
                return (sbyte)RandomNumberGenerator.GetBytes(1)[0];
        }
    }
}