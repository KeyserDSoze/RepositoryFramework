using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.Json;
using Blazorise.DataGrid;

namespace RepositoryFramework.Web.Components
{
    public sealed class EntitiesTypeManager
    {
        private readonly ConcurrentDictionary<Type, EntityType> _trees = new();
        public EntityType GetEntity<T>(T? entity)
            => GetEntity(entity?.GetType() ?? typeof(T));
        public EntityType GetEntity(Type type)
        {
            if (!_trees.ContainsKey(type))
                _trees.TryAdd(type, new EntityType(type));
            return _trees[type];
        }
    }
    public sealed class EntityType : PropertyTree
    {
        public EntityType(Type type) : base(type, null)
        {
        }
    }
    public class PropertyTree
    {
        public List<PropertyInfoKeeper> Primitives { get; } = new();
        public List<PropertyTree> Enumerables { get; } = new();
        public List<PropertyTree> Complexes { get; } = new();
        public List<PropertyInfoKeeper> AllProperties { get; } = new();
        public PropertyInfoKeeper PropertyInfo { get; }
        public PropertyTree(Type type, PropertyInfoKeeper? from)
        {
            PropertyInfo = from;
            CheckType(type, from);
        }
        public void CheckType(Type type, PropertyInfoKeeper? from)
        {
            foreach (var property in type.FetchProperties())
            {
                var actualProperty = from.Next(property);
                if (actualProperty.IsPrimitive)
                {
                    Primitives.Add(actualProperty);
                    AllProperties.Add(actualProperty);
                }
                else if (actualProperty.IsEnumerable)
                {
                    var enumerable = property.PropertyType.GetInterfaces().FirstOrDefault(x => x.Name.StartsWith("IEnumerable`1"));
                    var generics = enumerable?.GetGenericArguments();
                    if (generics != null)
                    {
                        AllProperties.Add(actualProperty);
                        Enumerables.Add(new PropertyTree(generics.First(), actualProperty));
                    }
                }
                else
                {
                    Complexes.Add(new PropertyTree(property.PropertyType, actualProperty));
                    CheckType(property.PropertyType, actualProperty);
                }
            }
        }
    }
    public class PropertyInfoKeeper
    {
        public List<PropertyInfo> NavigationProperties { get; init; } = new();
        public PropertyInfo PropertyInfo { get; init; } = null!;
        public string NavigationPath => NavigationProperties.Count > 0 ? $"{string.Join('.', NavigationProperties.Select(x => x.Name))}.{PropertyInfo.Name}" : PropertyInfo.Name;
        public string Name => PropertyInfo.Name;
        public bool IsEnumerable { get; init; }
        public bool IsPrimitive { get; init; }
        public object? Value(object? context)
        {
            if (context == null)
                return null;
            foreach (var item in NavigationProperties)
            {
                context = item.GetValue(context);
                if (context == null)
                    return null;
            }
            context = PropertyInfo.GetValue(context);
            return context;
        }
        public void Set(object? context, object? value)
        {
            if (context == null || value == null)
                return;
            foreach (var item in NavigationProperties)
            {
                context = item.GetValue(context);
                if (context == null)
                    return;
            }
            PropertyInfo.SetValue(context, value.Cast(PropertyInfo.PropertyType));
        }

    }
    public static class PropertyInfoKeeperExtensions
    {
        public static PropertyInfoKeeper Next(this PropertyInfoKeeper? keeper, PropertyInfo propertyInfo)
        {
            var newInfo = new PropertyInfoKeeper
            {
                PropertyInfo = propertyInfo,
                IsEnumerable = propertyInfo.PropertyType.GetInterface(nameof(IEnumerable)) != null,
                IsPrimitive = propertyInfo.PropertyType.IsPrimitive()
            };
            if (keeper != null)
                foreach (var navigationProperty in keeper.NavigationProperties)
                    newInfo.NavigationProperties.Add(navigationProperty);
            return newInfo;
        }
    }

    internal sealed class PropertyInfoKeeper<T, TKey>
        where TKey : notnull
    {
        public List<PropertyInfo> NavigationProperties { get; init; } = new();
        public PropertyInfo PropertyInfo { get; init; } = null!;
        public object? Value(object? context)
        {
            if (context == null)
                return null;
            foreach (var item in NavigationProperties)
            {
                context = item.GetValue(context);
                if (context == null)
                    return null;
            }
            context = PropertyInfo.GetValue(context);
            return context;
        }
        public IEnumerable AsEnumerable(object? context)
            => (Value(context) as IEnumerable)!;
        public string Count(object? context)
        {
            var enumerable = AsEnumerable(context);
            if (enumerable is IList listable)
                return $"{listable.Count} items.";
            else
                return $"{enumerable.AsQueryable().Count()} items.";
        }
        public void Set(object? context, string? value)
        {
            if (context == null || value == null)
                return;
            foreach (var item in NavigationProperties)
            {
                context = item.GetValue(context);
                if (context == null)
                    return;
            }
            PropertyInfo.SetValue(context, value);
        }
        public void SetJson(CellEditContext<Entity<T, TKey>> context, string? value)
            => context.CellValue = value?.FromJson(PropertyInfo.PropertyType);
        public string Name { get; init; } = null!;
        public string Label { get; init; } = null!;
    }
}
