using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class NumberPopulationService<T, TKey> : INumberPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {
            if (type == typeof(int) || type == typeof(int?))
                return BitConverter.ToInt32(RandomNumberGenerator.GetBytes(4));
            else if (type == typeof(uint) || type == typeof(uint?))
                return BitConverter.ToUInt32(RandomNumberGenerator.GetBytes(4));
            else if (type == typeof(short) || type == typeof(short?))
                return BitConverter.ToInt16(RandomNumberGenerator.GetBytes(2));
            else if (type == typeof(ushort) || type == typeof(ushort?))
                return BitConverter.ToUInt16(RandomNumberGenerator.GetBytes(2));
            else if (type == typeof(long) || type == typeof(long?))
                return BitConverter.ToInt64(RandomNumberGenerator.GetBytes(8));
            else if (type == typeof(ulong) || type == typeof(ulong?))
                return BitConverter.ToUInt64(RandomNumberGenerator.GetBytes(8));
            else if (type == typeof(nint) || type == typeof(nint?))
                return (nint)BitConverter.ToInt16(RandomNumberGenerator.GetBytes(2));
            else if (type == typeof(nuint) || type == typeof(nuint?))
                return (nuint)BitConverter.ToUInt16(RandomNumberGenerator.GetBytes(2));
            else if (type == typeof(float) || type == typeof(float?))
                return BitConverter.ToSingle(RandomNumberGenerator.GetBytes(4));
            else if (type == typeof(double) || type == typeof(double?))
                return BitConverter.ToDouble(RandomNumberGenerator.GetBytes(8));
            else
                return new decimal(BitConverter.ToInt32(RandomNumberGenerator.GetBytes(4)),
                    BitConverter.ToInt32(RandomNumberGenerator.GetBytes(4)),
                    BitConverter.ToInt32(RandomNumberGenerator.GetBytes(4)),
                    RandomNumberGenerator.GetInt32(4) > 1,
                    (byte)RandomNumberGenerator.GetInt32(29));
        }
    }
}