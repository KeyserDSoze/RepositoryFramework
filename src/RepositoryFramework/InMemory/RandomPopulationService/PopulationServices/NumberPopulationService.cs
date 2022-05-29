using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class NumberPopulationService : IRandomPopulationService
    {
        public int Priority => 1;
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
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

        public bool IsValid(Type type)
            => type == typeof(int) || type == typeof(int?) || type == typeof(uint) || type == typeof(uint?)
                || type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?)
                || type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?)
                || type == typeof(nint) || type == typeof(nint?) || type == typeof(nuint) || type == typeof(nuint?)
                || type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?)
                || type == typeof(decimal) || type == typeof(decimal?);
    }
}