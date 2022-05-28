namespace RepositoryFramework.Population
{
    internal class ArrayPopulationService<T, TKey> : IArrayPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {
            var entity = Activator.CreateInstance(type, numberOfEntities);
            var valueType = type.GetElementType();
            for (int i = 0; i < numberOfEntities; i++)
                (entity as dynamic)![i] = populationService.Construct(valueType!, numberOfEntities, treeName, string.Empty);
            return entity!;
        }
    }
}