using System.Collections;
using System.Collections.Concurrent;
using System.IO;
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
        public PropertyInfo? PropertyInfo { get; }
        public List<PropertyInfoKeeper> Primitives { get; }
        public List<PropertyTree> Enumerables { get; }
        public List<PropertyTree> Complexes { get; }
        public PropertyTree(Type type, string? path)
        {
            Primitives = type.FetchProperties()
                .Where(x => x.PropertyType.IsPrimitive())
                .Select(x => new PropertyInfoKeeper
                {
                    PropertyInfo = x,
                    Name = !string.IsNullOrWhiteSpace(path) ? $"{path}.{x.Name}" : x.Name,
                    Label = x.Name,
                }).ToList();
            Complexes = type.FetchProperties()
                .Where(x => !x.PropertyType.IsPrimitive() && x.PropertyType.GetInterface(nameof(IEnumerable)) == null)
            .Select(x => new PropertyTree(x.PropertyType, !string.IsNullOrWhiteSpace(path) ? $"{path}.{x.Name}" : x.Name))
            .ToList();
            Enumerables = type.FetchProperties()
                .Where(x => !x.PropertyType.IsPrimitive() && x.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                .Select(x => new PropertyTree(x.PropertyType, !string.IsNullOrWhiteSpace(path) ? $"{path}.{x.Name}" : x.Name))
                .ToList();
        }
    }
    public sealed class PropertyInfoKeeper
    {
        public List<PropertyInfo> NavigationProperties { get; init; } = new();
        public PropertyInfo PropertyInfo { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Label { get; init; } = null!;
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
