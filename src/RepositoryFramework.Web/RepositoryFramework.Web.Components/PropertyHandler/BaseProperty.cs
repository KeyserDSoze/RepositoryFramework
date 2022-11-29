using System.Linq.Dynamic.Core;
using System.Reflection;

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
        private readonly List<PropertyInfo> _valueFromContextStack = new();
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
            context = Value(context, true);
            if (context != null)
                Self.SetValue(context, value);
        }
    }
}
