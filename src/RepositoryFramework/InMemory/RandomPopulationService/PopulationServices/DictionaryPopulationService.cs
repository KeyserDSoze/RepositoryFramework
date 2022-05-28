using System.Collections;

namespace RepositoryFramework.Population
{
    internal class DictionaryPopulationService<T, TKey> : IDictionaryPopulationService<T, TKey>
        where TKey : notnull
    { 
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {
            var keyType = type.GetGenericArguments().First();
            var valueType = type.GetGenericArguments().Last();
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var entity = Activator.CreateInstance(dictionaryType)! as IDictionary;
            for (int i = 0; i < numberOfEntities; i++)
            {
                var newKey = populationService.Construct(type.GetGenericArguments().First(), numberOfEntities, treeName, "Key");
                var newValue = populationService.Construct(type.GetGenericArguments().Last(), numberOfEntities, treeName, "Value");
                entity!.Add(newKey, newValue);
            }
            return entity!;
        }
    }
}