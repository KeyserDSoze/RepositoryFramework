using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    internal class ObjectPopulationService : IRandomPopulationService
    {
        public int Priority => 0;

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