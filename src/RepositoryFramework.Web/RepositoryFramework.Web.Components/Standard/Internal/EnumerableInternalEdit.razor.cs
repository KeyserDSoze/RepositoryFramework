using System.Reflection;
using Blazorise;
using Blazorise.Extensions;
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
        public IEnumerator<string> ColorEnumerator { get; set; }
        [Inject]
        public PropertyHandler PropertyHandler { get; set; } = null!;
        [Inject]
        public AppSettings AppSettings { get; set; } = null!;
        private TypeShowcase TypeShowcase { get; set; } = null!;
        protected override Task OnParametersSetAsync()
        {
            if (Entities == null)
                Entities = new List<T>();
            if (AppSettings.Palette == AppPalette.Pastels)
            {
                ColorEnumerator ??= Constant.Color.GetPastels().GetEnumerator();
            }
            if (!typeof(T).IsPrimitive())
                TypeShowcase = PropertyHandler.GetEntity(typeof(T));
            return base.OnParametersSetAsync();
        }
        private protected RenderFragment LoadNext(BaseProperty property)
        {
            if (property.Type == PropertyType.Complex)
            {
                var value = property.Value(Entities);
                var genericType = typeof(InternalEdit<>).MakeGenericType(new[] { property.Self.PropertyType });
                var frag = new RenderFragment(b =>
                {
                    b.OpenComponent(1, genericType);
                    b.AddAttribute(2, Constant.Entity, value);
                    b.AddAttribute(3, Constant.ColorEnumerator, ColorEnumerator);
                    b.AddAttribute(4, Constant.DisableEdit, DisableEdit);
                    b.CloseComponent();
                });
                return frag;
            }
            else
            {
                return default!;
            }
        }
        private RenderFragment LoadPrimitiveEdit(T? entity, int index)
        {
            var genericType = typeof(InternalPrimitiveEdit<T>);
            var frag = new RenderFragment(b =>
            {
                b.OpenComponent(1, genericType);
                b.AddAttribute(2, Constant.Name, $"{index + 1}.");
                b.AddAttribute(3, Constant.Value, entity);
                b.AddAttribute(4, Constant.Update, (object x) => Update(index, (T)x));
                b.AddAttribute(5, Constant.DisableEdit, DisableEdit);
                b.CloseComponent();
            });
            return frag;
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
        public void Delete(T entity)
        {
            if (entity != null)
            {
                if (Entities is ICollection<T> collection)
                {
                    collection.Remove(entity);
                }
                else if (Entities is T[] array)
                {
                    var newArray = new T[array.Length - 1];
                    var counter = 0;
                    foreach (var element in array)
                    {
                        if (!entity.Equals(element))
                        {
                            newArray[counter] = element;
                            counter++;
                        }
                    }
                    Entities = newArray;
                    Property.Set(Context, newArray);
                }
            }
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
