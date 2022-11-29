using System.Collections.Concurrent;

namespace RepositoryFramework.Web.Components
{
    public sealed class PropertyHandler
    {
        private readonly ConcurrentDictionary<Type, TypeShowcase> _trees = new();
        public TypeShowcase GetEntity<T>(T? entity)
            => GetEntity(entity?.GetType() ?? typeof(T));
        public TypeShowcase GetEntity(Type type)
        {
            if (!_trees.ContainsKey(type))
                _trees.TryAdd(type, new TypeShowcase(type));
            return _trees[type];
        }
    }
}
