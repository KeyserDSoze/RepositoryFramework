﻿using System.Reflection;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class EnumerableInternalEdit<T>
    {
        [Parameter]
        public IEnumerable<T> Entities { get; set; } = null!;
        [Parameter]
        public BaseProperty Property { get; set; } = null!;
        [Parameter]
        public object? Context { get; set; }
        [Parameter]
        public bool DisableEdit { get; set; }
        [Parameter]
        public Dictionary<string, PropertyUiSettings>? PropertiesUiSettings { get; set; }
        [Parameter]
        public int Deep { get; set; }
        [Parameter]
        public PropertyUiSettings PropertyUiSettings { get; set; }
        [Parameter]
        public string? NavigationPath { get; set; }
        [Parameter]
        public string? Error { get; set; }
        [CascadingParameter]
        public EditParameterBearer EditParameterBearer { get; set; }
        [Inject]
        public PropertyHandler PropertyHandler { get; set; } = null!;
        private TypeShowcase TypeShowcase { get; set; } = null!;
        private string? GetNextNavigationPath()
        {
            var navigationPath = NavigationPath;
            if (NavigationPath != null)
                navigationPath = $"{NavigationPath}.{Property.Self.Name}";
            return navigationPath;
        }
        protected override Task OnParametersSetAsync()
        {
            if (Entities == null)
                Entities = new List<T>();
            if (!typeof(T).IsPrimitive())
                TypeShowcase = PropertyHandler.GetEntity(typeof(T));
            return base.OnParametersSetAsync();
        }
        public void Update(int index, T value)
        {
            if (Entities is IList<T> list)
                list[index] = value;
            else if (Entities is T[] array)
                array[index] = value;
            else if (Entities is ICollection<T> collection)
            {
                var element = collection.ElementAt(index);
                collection.Remove(element);
                collection.Add(value);
            }
        }
        public void Update(IEnumerable<T> values)
        {
            if (Entities is IList<T> list)
            {
                list.Clear();
                foreach (var value in values)
                    list.Add(value);
            }
            else if (Entities is T[])
            {
                Property.Set(Context, values.ToArray());
            }
            else if (Entities is ICollection<T> collection)
            {
                collection.Clear();
                foreach (var value in values)
                    collection.Add(value);
            }
        }
        public void Delete(int index)
        {
            InvokeAsync(() =>
            {
                if (Entities is ICollection<T> collection)
                {
                    var entity = Entities.Skip(index).FirstOrDefault();
                    if (entity != null)
                        collection.Remove(entity);
                }
                else if (Entities is T[] array)
                {
                    var newArray = new T[array.Length - 1];
                    var counter = 0;
                    for (var i = 0; i < array.Length; i++)
                    {
                        if (i != index)
                        {
                            newArray[counter] = array[i];
                            counter++;
                        }
                    }
                    Entities = newArray;
                    Property.Set(Context, newArray);
                }
                StateHasChanged();
            });
        }
        public void New()
        {
            var entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>()!;
            if (Entities is ICollection<T> collection)
            {
                collection.Add(entity);
            }
            else if (Entities is T[] array)
            {
                var newArray = new T[array.Length + 1];
                var counter = 0;
                foreach (var element in array)
                {
                    newArray[counter] = element;
                    counter++;
                }
                newArray[counter] = entity;
                Entities = newArray;
                Property.Set(Context, newArray);
            }
        }
    }
}
