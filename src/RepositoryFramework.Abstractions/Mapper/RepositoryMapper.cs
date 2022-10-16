using System.Collections;
using System.Reflection;

namespace RepositoryFramework
{
    internal sealed class RepositoryMapper<T, TKey, TEntityModel> : IRepositoryMapper<T, TKey, TEntityModel>
        where TKey : notnull
    {
        internal sealed record RepositoryMapperProperty
            (Func<T, dynamic> GetFromT, Action<T, dynamic> SetToT,
            Func<TEntityModel, dynamic> GetFromTEntityModel, Action<TEntityModel, dynamic> SetToTEntityModel);
        internal sealed record RepositoryKeyMapperProperty
            (Func<TKey, dynamic> GetFromKey, Action<TKey, dynamic>? SetToKey,
            Func<TEntityModel, dynamic> GetFromTEntityModel, Action<TEntityModel, dynamic> SetToTEntityModel);
        public static RepositoryMapper<T, TKey, TEntityModel> Instance { get; } = new();
        internal List<RepositoryMapperProperty> Properties { get; } = new();
        internal List<RepositoryKeyMapperProperty> KeyProperties { get; } = new();
        private RepositoryMapper() { }
        public T? Map(TEntityModel? entity)
        {
            if (entity == null)
                return default;
            var t = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
            if (t is null)
                return default;
            foreach (var property in Properties)
            {
                var valueFromEntityModel = property.GetFromTEntityModel(entity);
                if (valueFromEntityModel is not string && valueFromEntityModel is IEnumerable enumerable)
                {
                    var valueFromT = property.GetFromT(t);
                    if (valueFromT is IEnumerable tEnumerable)
                    {
                        var dynamicEnumerable = (dynamic)tEnumerable;
                        foreach (var item in enumerable)
                            dynamicEnumerable.Add(item);
                    }
                }
                else
                    property.SetToT(t, valueFromEntityModel);
            }
            return t;
        }

        public TEntityModel? Map(T? entity, TKey key)
        {
            if (entity == null)
                return default;
            var entityModel = typeof(TEntityModel).CreateWithDefaultConstructorPropertiesAndField<TEntityModel>();
            if (entityModel is null)
                return default;
            foreach (var property in Properties)
            {
                var valueFromT = property.GetFromT(entity);
                if (valueFromT is not string && valueFromT is IEnumerable enumerable)
                {
                    var valueFromEntityModel = property.GetFromTEntityModel(entityModel);
                    if (valueFromEntityModel is IEnumerable tEnumerable)
                    {
                        var dynamicEnumerable = (dynamic)tEnumerable;
                        foreach (var item in enumerable)
                            dynamicEnumerable.Add(item);
                    }
                }
                else
                    property.SetToTEntityModel(entityModel, valueFromT);
            }
            return entityModel;
        }

        public TKey? RetrieveKey(TEntityModel? entity)
        {
            if (entity == null)
                return default;
            var key = typeof(TKey).CreateWithDefaultConstructorPropertiesAndField<TKey>();
            if (key is null)
                return default;
            if (KeyProperties.Any(x => x.SetToKey == null))
                return (TKey)KeyProperties.First().GetFromTEntityModel(entity);
            foreach (var property in KeyProperties)
            {
                var valueFromEntityModel = property.GetFromTEntityModel(entity);
                if (valueFromEntityModel is not string && valueFromEntityModel is IEnumerable enumerable)
                {
                    var valueFromKey = property.GetFromKey(key);
                    if (valueFromKey is IEnumerable tEnumerable)
                    {
                        var dynamicEnumerable = (dynamic)tEnumerable;
                        foreach (var item in enumerable)
                            dynamicEnumerable.Add(item);
                    }
                }
                else
                    property.SetToKey(key, valueFromEntityModel);
            }
            return key;
        }
    }
}
