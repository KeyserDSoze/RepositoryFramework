using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class EnumerableInternalEdit<T>
    {
        [Parameter]
        public IEnumerable<T> Entities { get; set; }
        [Inject]
        public PropertyBringer PropertyBringer { get; set; }
        private TypeShowcase TypeShowcase { get; set; }
        protected override Task OnParametersSetAsync()
        {
            if (Entities == null)
                Entities = new List<T>();
            if (!typeof(T).IsPrimitive())
                TypeShowcase = PropertyBringer.GetEntity(typeof(T));
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
                    b.AddAttribute(2, nameof(Entity), value);
                    b.CloseComponent();
                });
                return frag;
            }
            else
            {
                return default;
            }
        }
        public void Update(int index, T value)
        {
            if (Entities is IList<T> list)
                list[index] = value;
        }
        public void Delete(T entity)
        {
            if (Entities is ICollection<T> collection)
            {
                collection.Remove(entity);
            }
        }
    }
}
