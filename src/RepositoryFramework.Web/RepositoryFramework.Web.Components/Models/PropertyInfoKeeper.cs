using System.Collections;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.Json;
using Blazorise.DataGrid;

namespace RepositoryFramework.Web.Components
{
    internal sealed class PropertyInfoKeeper
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
