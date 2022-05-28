using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    internal class ObjectPopulationService : IClassPopulationService
    {
        private readonly IInstanceCreator _instanceCreator;

        public ObjectPopulationService(IInstanceCreator instanceCreator)
        {
            _instanceCreator = instanceCreator;
        }
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
        {
            if (!type.IsInterface && !type.IsAbstract)
            {
                var entity = _instanceCreator.CreateInstance(type, populationService, numberOfEntities, treeName, settings);
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
    }
}