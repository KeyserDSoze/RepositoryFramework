using System.Collections;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class EnumerableInternalEdit
    {
        [CascadingParameter]
        public EditParametersBearer EditParametersBearer { get; set; }
        [Parameter]
        public BaseProperty BaseProperty { get; set; } = null!;

        private IEnumerable _entities;
        private string? _error;
        private PropertyUiSettings? _propertyUiSettings;
        protected override void OnParametersSet()
        {
            var retrieveEntities = EditParametersBearer.GetValue(BaseProperty);
            if (retrieveEntities.Exception == null)
            {
                _entities = retrieveEntities.Entity as IEnumerable;
                _propertyUiSettings = EditParametersBearer.GetSettings(BaseProperty);
            }
            else
                _error = retrieveEntities.Exception.Message;
            base.OnParametersSet();
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
                BaseProperty.Set(Context, values.ToArray());
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
                    BaseProperty.Set(Context, newArray);
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
                BaseProperty.Set(Context, newArray);
            }
        }
    }
}
