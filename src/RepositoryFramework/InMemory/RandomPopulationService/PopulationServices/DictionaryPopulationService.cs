using System.Collections;

namespace RepositoryFramework.Population
{
    internal class DictionaryPopulationService : IRandomPopulationService
    {
        public int Priority => 3;

        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
        {
            var keyType = type.GetGenericArguments().First();
            var valueType = type.GetGenericArguments().Last();
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var entity = Activator.CreateInstance(dictionaryType)! as IDictionary;
            for (int i = 0; i < numberOfEntities; i++)
            {
                var newKey = populationService.Construct(type.GetGenericArguments().First(), numberOfEntities, treeName, "Key", settings);
                var newValue = populationService.Construct(type.GetGenericArguments().Last(), numberOfEntities, treeName, "Value", settings);
                entity!.Add(newKey, newValue);
            }
            return entity!;
        }

        public bool IsValid(Type type)
        {
            if (!type.IsArray)
            {
                var interfaces = type.GetInterfaces();
                if (type.Name.Contains("IDictionary`2") || interfaces.Any(x => x.Name.Contains("IDictionary`2")))
                    return true;
            }
            return false;
        }
    }
}