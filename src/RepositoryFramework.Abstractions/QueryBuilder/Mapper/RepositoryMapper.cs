using System.Reflection;

namespace RepositoryFramework
{
    internal sealed class RepositoryMapper<T, TKey, TEntityModel> : IRepositoryMapper<T, TKey, TEntityModel>
        where TKey : notnull
    {
        internal sealed record RepositoryMapperProperty
            (Func<T, dynamic> GetFromT, Action<T, dynamic> SetToT,
            Func<TEntityModel, dynamic> GetFromTEntityModel, Action<TEntityModel, dynamic> SetToTEntityModel);
        public static RepositoryMapper<T, TKey, TEntityModel> Instance { get; } = new();
        internal List<RepositoryMapperProperty> Properties { get; } = new();
        private RepositoryMapper() { }
        public T? Map(TEntityModel? entity)
        {
            if (entity == null)
                return default;
            var t = (T)typeof(T).CreateInstance();
            foreach (var property in Properties)
                property.SetToT(t, property.GetFromTEntityModel(entity));
            return t;
        }

        public TEntityModel? Map(T? entity, TKey key)
        {
            if (entity == null)
                return default;
            var entityModel = (TEntityModel)typeof(TEntityModel).CreateInstance();
            foreach (var property in Properties)
                property.SetToTEntityModel(entityModel, property.GetFromT(entity));
            return entityModel;
        }

        public TKey? RetrieveKey(TEntityModel? entity)
        {
            throw new NotImplementedException();
        }
    }
}
