using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Model for your repository service registry.
    /// </summary>
    public class RepositoryFrameworkService
    {
        public Dictionary<string, (Type InterfaceType, Type CurrentType)> RepositoryTypes { get; }
        public Type KeyType { get; }
        public Type ModelType { get; }
        public bool IsNotExposable { get; internal set; }
        public ServiceLifetime ServiceLifetime { get; internal set; }
        public RepositoryFrameworkService(Type keyType, Type modelType)
        {
            KeyType = keyType;
            ModelType = modelType;
            RepositoryTypes = new();
        }
        public void AddOrUpdate(Type interfaceType, Type currentType)
        {
            if (RepositoryTypes.ContainsKey(interfaceType.Name))
                RepositoryTypes[interfaceType.Name] = (interfaceType, currentType);
            else
                RepositoryTypes.Add(interfaceType.Name, (interfaceType, currentType));
        }
    }
}
