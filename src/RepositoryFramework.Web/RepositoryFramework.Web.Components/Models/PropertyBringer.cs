using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace RepositoryFramework.Web.Components
{
    public sealed class PropertyBringer
    {
        private readonly ConcurrentDictionary<Type, TypeShowcase> _trees = new();
        public TypeShowcase GetEntity<T>(T? entity)
            => GetEntity(entity?.GetType() ?? typeof(T));
        public TypeShowcase GetEntity(Type type)
        {
            if (!_trees.ContainsKey(type))
                _trees.TryAdd(type, new TypeShowcase(type));
            return _trees[type];
        }
    }
    public enum PropertyType
    {
        Primitive,
        Complex,
        Enumerable
    }
    public sealed class TypeShowcase
    {
        public List<BaseProperty> Properties { get; } = new();
        public TypeShowcase(Type type)
        {
            foreach (var property in type.FetchProperties())
                Properties.Add(PropertyStrategy.Instance.CreateProperty(property, null));
        }
        public IEnumerable<BaseProperty> GetAllPrimiviteProperties()
        {
            foreach (var property in Properties)
                foreach (var x in property.GetQueryableProperty())
                    yield return x;
        }
    }
    public abstract class BaseProperty
    {
        public BaseProperty? Father { get; }
        public PropertyInfo Self { get; }
        public PropertyType Type { get; private protected set; }
        public List<BaseProperty> Sons { get; } = new();
        private readonly List<PropertyInfo> _valueFromContextStack = new();
        public Type[] Generics { get; private protected set; }
        public string NavigationPath { get; }
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
    public sealed class PrimitiveProperty : BaseProperty
    {
        public PrimitiveProperty(PropertyInfo info, BaseProperty? father) : base(info, father)
        {
            Type = PropertyType.Primitive;
        }

        public override IEnumerable<BaseProperty> GetQueryableProperty()
        {
            yield return this;
        }

        protected override void ConstructWell()
        {

        }
    }
    public sealed class EnumerableProperty : BaseProperty
    {
        public EnumerableProperty(PropertyInfo info, BaseProperty? father) : base(info, father)
        {
            Type = PropertyType.Enumerable;
        }

        public override IEnumerable<BaseProperty> GetQueryableProperty()
        {
            yield return this;
        }

        protected override void ConstructWell()
        {
            var enumerable = Self.PropertyType.GetInterfaces().FirstOrDefault(x => x.Name.StartsWith("IEnumerable`1"));
            Generics = enumerable?.GetGenericArguments();
            if (Generics != null)
                foreach (var generic in Generics)
                {
                    if (!generic.IsPrimitive())
                        foreach (var property in generic.FetchProperties())
                            Sons.Add(PropertyStrategy.Instance.CreateProperty(property, this));
                }
        }
    }
    public sealed class ComplexProperty : BaseProperty
    {
        public ComplexProperty(PropertyInfo info, BaseProperty? father) : base(info, father)
        {
            Type = PropertyType.Complex;
        }

        public override IEnumerable<BaseProperty> GetQueryableProperty()
        {
            foreach (var property in Sons)
            {
                if (property.Type == PropertyType.Complex)
                    foreach (var deeperProperty in property.GetQueryableProperty())
                        yield return deeperProperty;
                else
                    yield return property;
            }
        }

        protected override void ConstructWell()
        {
            foreach (var property in Self.PropertyType.FetchProperties())
            {
                Sons.Add(PropertyStrategy.Instance.CreateProperty(property, this));
            }
        }
    }
    internal sealed class PropertyStrategy
    {
        public static PropertyStrategy Instance { get; } = new PropertyStrategy();
        private PropertyStrategy() { }
        public BaseProperty CreateProperty(PropertyInfo propertyInfo, BaseProperty? father)
        {
            if (propertyInfo.PropertyType.IsPrimitive())
                return new PrimitiveProperty(propertyInfo, father);
            else if (propertyInfo.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                return new EnumerableProperty(propertyInfo, father);
            else
                return new ComplexProperty(propertyInfo, father);
        }
    }
}
