using System.Linq.Dynamic.Core;
using System.Reflection;
using RepositoryFramework.Web.Components.Standard;

namespace RepositoryFramework.Web.Components
{
    public abstract class BaseProperty
    {
        public BaseProperty? Father { get; }
        public PropertyInfo Self { get; }
        public PropertyType Type { get; private protected set; }
        public List<BaseProperty> Sons { get; } = new();
        public Type[] Generics { get; private protected set; } = null!;
        public string NavigationPath { get; }
        public string Title { get; }
        public int Deep { get; }
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
        public object? Value(object? context, bool onlyContainer = false)
        {
            if (context == null)
                return null;
            foreach (var item in _valueFromContextStack.Take(_valueFromContextStack.Count - (onlyContainer ? 1 : 0)))
            {
                context = item.GetValue(context);
                if (context == null)
                    return null;
            }
            return context;
        }
        public void Set(object? context, object? value)
        {
            context = Try.WithDefaultOnCatch(() => Value(context, true)).Entity;
            if (context != null)
                Self.SetValue(context, value);
        }
    }
}
