using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    internal class ObjectPopulationService<T, TKey> : IClassPopulationService<T, TKey>
        where TKey : notnull
    {
        private readonly IInstanceCreator _instanceCreator;

        public ObjectPopulationService(IInstanceCreator instanceCreator)
        {
            _instanceCreator = instanceCreator;
        }
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {
            if (!type.IsInterface && !type.IsAbstract)
            {
                var entity = _instanceCreator.CreateInstance(type, populationService, numberOfEntities, treeName);
                try
                {
                    var properties = type.GetProperties();
                    foreach (var property in properties)
                        property.SetValue(entity, populationService.Construct(property.PropertyType, numberOfEntities, treeName, property.Name));
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