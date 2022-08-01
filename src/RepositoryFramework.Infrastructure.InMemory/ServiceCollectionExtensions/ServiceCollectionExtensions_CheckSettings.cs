using RepositoryFramework;
using RepositoryFramework.InMemory;
using RepositoryFramework.InMemory.Population;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static void CheckSettings<T, TKey>(RepositoryBehaviorSettings<T, TKey> settings)
             where TKey : notnull
        {
            Check(settings.Get(RepositoryMethods.Insert).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.Update).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.Delete).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.Batch).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.Get).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.Query).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.Exist).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.Count).ExceptionOdds);
            Check(settings.Get(RepositoryMethods.All).ExceptionOdds);

            static void Check(List<ExceptionOdds> odds)
            {
                var total = odds.Sum(x => x.Percentage);
                if (odds.Any(x => x.Percentage <= 0 || x.Percentage > 100))
                {
                    throw new ArgumentException("Some percentages are wrong, greater than 100% or lesser than 0.");
                }
                if (total > 100)
                    throw new ArgumentException("Your total percentage is greater than 100.");
            }
        }
    }
}