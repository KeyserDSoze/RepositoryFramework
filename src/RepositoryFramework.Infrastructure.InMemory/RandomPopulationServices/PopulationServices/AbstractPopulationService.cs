using System.Reflection;

namespace RepositoryFramework.InMemory.Population
{
    internal class AbstractPopulationService : IRandomPopulationService
    {
        public int Priority => 0;

        public dynamic GetValue(RandomPopulationOptions options)
        {
            try
            {
                return options.PopulationService.Construct(options.Type.Mock()!,
                    options.NumberOfEntities, options.TreeName, string.Empty)!;
            }
            catch
            {
                return null!;
            }
        }

        public bool IsValid(Type type)
            => type.IsAbstract;

    }
}