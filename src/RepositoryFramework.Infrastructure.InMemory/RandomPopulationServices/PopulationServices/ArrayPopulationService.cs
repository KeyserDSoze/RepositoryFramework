namespace RepositoryFramework.InMemory.Population
{
    internal class ArrayPopulationService : IRandomPopulationService
    {
        public int Priority => 1;
        public dynamic GetValue(RandomPopulationOptions options)
        {
            var entity = Activator.CreateInstance(options.Type, options.NumberOfEntities);
            var valueType = options.Type.GetElementType();
            for (int i = 0; i < options.NumberOfEntities; i++)
                (entity as dynamic)![i] = options.PopulationService.Construct(valueType!,
                    options.NumberOfEntities, options.TreeName, string.Empty);
            return entity!;
        }

        public bool IsValid(Type type) 
            => type.IsArray;
    }
}