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
    public class PropertyTree : PropertyInfoKeeper
    {
        public List<PropertyInfoKeeper> Primitives { get; } = new();
        public List<PropertyTree> Enumerables { get; } = new();
        public List<PropertyTree> Complexes { get; } = new();
        public List<PropertyInfoKeeper> AllProperties { get; } = new();
        public PropertyTree(Type type, string? navigationPath)
        {
            CheckType(type, navigationPath);
        }
        public void CheckType(Type type, string? navigationPath)
        {
            foreach (var property in type.FetchProperties())
            {
                if (property.PropertyType.IsPrimitive())
                {
                    Primitives.Add(new PropertyInfoKeeper
                    {
                        PropertyInfo = property,
                        NavigationPath = !string.IsNullOrWhiteSpace(navigationPath) ? $"{navigationPath}.{property.Name}" : property.Name,
                        Name = property.Name,
                        IsEnumerable = false,
                    });
                    AllProperties.Add(Primitives.Last());
                }
                else if (property.PropertyType.GetInterface(nameof(IEnumerable)) == null)
                {
                    Enumerables.Add(new PropertyTree(property.PropertyType,
                        !string.IsNullOrWhiteSpace(navigationPath) ? $"{navigationPath}.{property.Name}" : property.Name));
                    AllProperties.Add(new PropertyInfoKeeper
                    {
                        PropertyInfo = property,
                        NavigationPath = !string.IsNullOrWhiteSpace(navigationPath) ? $"{navigationPath}.{property.Name}" : property.Name,
                        Name = property.Name,
                        IsEnumerable = true,
                    });
                }
                else
                {
                    Complexes.Add(new PropertyTree(property.PropertyType,
                        !string.IsNullOrWhiteSpace(navigationPath) ? $"{navigationPath}.{property.Name}" : property.Name));
                    CheckType(property.PropertyType, Complexes.Last().NavigationPath);
                }
            }
        }
    }
    public class PropertyInfoKeeper
    {
        public List<PropertyInfo> NavigationProperties { get; init; } = new();
        public PropertyInfo PropertyInfo { get; init; } = null!;
        public string NavigationPath { get; init; } = null!;
        public string Name { get; init; } = null!;
        public bool IsEnumerable { get; init; }
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
