using System.Collections;

namespace RepositoryFramework.InMemory.Population
{
    internal class EnumerablePopulationService : IRandomPopulationService
    {
        public int Priority => 2;
        public dynamic GetValue(RandomPopulationOptions options)
        {
            var valueType = options.Type.GetGenericArguments().First();
            var listType = typeof(List<>).MakeGenericType(valueType);
            var entity = Activator.CreateInstance(listType)! as IList;
            for (var i = 0; i < options.NumberOfEntities; i++)
            {
                var newValue = options.PopulationService.Construct(options.Type.GetGenericArguments().First(),
                    options.NumberOfEntities, options.TreeName, string.Empty);
                entity!.Add(newValue);
            }
            return entity!;
        }

        public bool IsValid(Type type)
        {
            if (!type.IsArray)
            {
                var interfaces = type.GetInterfaces();
                if (type.Name.Contains("IEnumerable`1") || interfaces.Any(x => x.Name.Contains("IEnumerable`1")))
                    return true;
            }
            return false;
        }
    }
}
