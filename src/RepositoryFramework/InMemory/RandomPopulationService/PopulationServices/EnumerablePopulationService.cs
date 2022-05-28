using System.Collections;

namespace RepositoryFramework.Population
{
    internal class EnumerablePopulationService<T, TKey> : IEnumerablePopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {
            var valueType = type.GetGenericArguments().First();
            var listType = typeof(List<>).MakeGenericType(valueType);
            var entity = Activator.CreateInstance(listType)! as IList;
            for (int i = 0; i < numberOfEntities; i++)
            {
                var newValue = populationService.Construct(type.GetGenericArguments().First(), numberOfEntities, treeName, string.Empty);
                entity!.Add(newValue);
            }
            return entity!;
        }
    }
}