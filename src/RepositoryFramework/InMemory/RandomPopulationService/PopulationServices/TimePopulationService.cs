using System.Security.Cryptography;

namespace RepositoryFramework.Population
{
    internal class TimePopulationService : ITimePopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
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