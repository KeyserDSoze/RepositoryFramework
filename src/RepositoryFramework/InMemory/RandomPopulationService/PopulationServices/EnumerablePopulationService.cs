using System.Collections;

namespace RepositoryFramework.Population
{
    internal class EnumerablePopulationService : IEnumerablePopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
        {
            var valueType = type.GetGenericArguments().First();
            var listType = typeof(List<>).MakeGenericType(valueType);
            var entity = Activator.CreateInstance(listType)! as IList;
            for (int i = 0; i < numberOfEntities; i++)
            {
                var newValue = populationService.Construct(type.GetGenericArguments().First(), numberOfEntities, treeName, string.Empty, settings);
                entity!.Add(newValue);
            }
            return entity!;
        }
    }
}