using System.Collections;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace RepositoryFramework.Web.Components
{
    public abstract class BaseProperty
    {
        public BaseProperty? Father { get; }
        public PropertyInfo Self { get; }
        public PropertyType Type { get; private protected set; }
        public PropertyType GenericType { get; }
        public List<BaseProperty> Sons { get; } = new();
        public Type[]? Generics { get; private protected set; }
        public string NavigationPath { get; }
        public string Title { get; }
        public int Deep { get; }
        public int EnumerableDeep { get; private protected set; }
        public Type AssemblyType => Self.PropertyType;
        private readonly List<PropertyInfo> _valueFromContextStack = new();
        public List<BaseProperty> Primitives { get; }
        public List<BaseProperty> NonPrimitives { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1699:Constructors should only call non-overridable methods", Justification = "Needed for logic flow.")]
        protected BaseProperty(PropertyInfo info, BaseProperty? father)
        {
            Self = info;
            Father = father;
            ConstructWell();
            if (Generics?.Length > 0)
                GenericType = Generics.First().IsPrimitive() ? PropertyType.Primitive : PropertyType.Complex;
            father = Father;
            _valueFromContextStack.Add(Self);
            while (father != null)
            {
                _valueFromContextStack.Add(father.Self);
                father = father.Father;
            }
            _valueFromContextStack.Reverse();
            NavigationPath = string.Join('.', _valueFromContextStack.Select(x => x.Name));
            Deep = NavigationPath.Split('.').Length;
            if (NavigationPath.StartsWith(Constant.ValueWithSeparator))
                Title = NavigationPath.Replace(Constant.ValueWithSeparator, string.Empty, 1);
            else
                Title = NavigationPath;
            Primitives = Sons.Where(x => x.Type == PropertyType.Primitive).ToList();
            NonPrimitives = Sons.Where(x => x.Type != PropertyType.Primitive).ToList();
        }
        protected abstract void ConstructWell();
        public abstract IEnumerable<BaseProperty> GetQueryableProperty();
        public object? Value(object? context, int[]? indexes)
        {
            if (context == null)
                return null;
            int counter = 0;
            foreach (var item in _valueFromContextStack)
            {
                context = item.GetValue(context);
                if (indexes != null && context is not string && context is IEnumerable enumerable)
                {
                    context = enumerable.ElementAt(indexes[counter]);
                    counter++;
                }
                if (context == null)
                    return null;
            }
            return context;
        }
        public void Set(object? context, object? value)
        {
            if (context == null)
                return;
            foreach (var item in _valueFromContextStack.Take(_valueFromContextStack.Count - 1))
            {
                context = item.GetValue(context);
                if (context == null)
                    return;
            }
            if (context != null)
                Self.SetValue(context, value);
        }
    }
}
