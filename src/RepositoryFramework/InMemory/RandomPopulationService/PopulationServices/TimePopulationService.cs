using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class TimePopulationService<T, TKey> : ITimePopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {
            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return DateTime.UtcNow;
            else if (type == typeof(TimeSpan) || type == typeof(TimeSpan?))
                return TimeSpan.FromTicks(RandomNumberGenerator.GetInt32(200_000));
            else
                return DateTimeOffset.UtcNow;
        }
    }
}