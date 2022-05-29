using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    internal class ObjectPopulationService : IRandomPopulationService
    {
        public int Priority => 0;

        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
        {
            if (!type.IsInterface && !type.IsAbstract)
            {
                var entity = populationService.InstanceCreator.CreateInstance(type, populationService, numberOfEntities, treeName, settings);
                try
                {
                    var properties = type.GetProperties();
                    foreach (var property in properties)
                        property.SetValue(entity, populationService.Construct(property.PropertyType, numberOfEntities, treeName, property.Name, settings));
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