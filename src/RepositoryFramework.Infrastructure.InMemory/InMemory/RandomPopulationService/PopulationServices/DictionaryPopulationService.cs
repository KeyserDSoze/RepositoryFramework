using System.Collections;

namespace RepositoryFramework.Population
{
    internal class DictionaryPopulationService : IRandomPopulationService
    {
        public int Priority => 3;

        public dynamic GetValue(RandomPopulationOptions options)
        {
            var keyType = options.Type.GetGenericArguments().First();
            var valueType = options.Type.GetGenericArguments().Last();
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var entity = Activator.CreateInstance(dictionaryType)! as IDictionary;
            for (int i = 0; i < options.NumberOfEntities; i++)
            {
                var newKey = options.PopulationService.Construct(options.Type.GetGenericArguments().First(),
                    options.NumberOfEntities, options.TreeName, "Key");
                var newValue = options.PopulationService.Construct(options.Type.GetGenericArguments().Last(),
                    options.NumberOfEntities, options.TreeName, "Value");
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