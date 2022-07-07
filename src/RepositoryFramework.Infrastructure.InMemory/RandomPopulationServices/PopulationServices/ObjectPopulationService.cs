namespace RepositoryFramework.InMemory.Population
{
    internal class ObjectPopulationService : IRandomPopulationService
    {
        public int Priority => 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "I need this behavior because I don't want to stop the creation of a random object for one not allowed strange type.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "I need this behavior because I don't want to stop the creation of a random object for one not allowed strange type.")]
        public dynamic GetValue(RandomPopulationOptions options)
        {
            if (!options.Type.IsInterface && !options.Type.IsAbstract)
            {
                var entity = options.PopulationService.InstanceCreator
                    .CreateInstance(options);
                try
                {
                    var properties = options.Type.GetProperties();
                    foreach (var property in properties)
                        property.SetValue(entity, options.PopulationService
                            .Construct(property.PropertyType, options.NumberOfEntities,
                            options.TreeName, property.Name));
                }
                catch
                {
                }
                return entity!;
            }
            return default!;
        }

        public bool IsValid(Type type)
            => !type.IsInterface && !type.IsAbstract;
    }
}