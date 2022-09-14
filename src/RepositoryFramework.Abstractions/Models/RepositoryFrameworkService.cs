namespace RepositoryFramework
{
    /// <summary>
    /// Model for your repository service registry.
    /// </summary>
    public class RepositoryFrameworkService
    {
        public bool NotExposableAsApi { get; internal set; }
        public Dictionary<string, (Type InterfaceType, Type CurrentType)> RepositoryTypes { get; }
        public Type KeyType { get; }
        public Type ModelType { get; }
        public RepositoryFrameworkService(Type keyType, Type modelType)
        {
            KeyType = keyType;
            ModelType = modelType;
            NotExposableAsApi = false;
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