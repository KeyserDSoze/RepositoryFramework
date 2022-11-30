namespace RepositoryFramework.Web.Components
{
    internal static class Constant
    {
        public const string Key = nameof(Key);
        public const string Entity = nameof(Entity);
        public const string Entities = nameof(Entities);
        public const string ColorEnumerator = nameof(ColorEnumerator);
        public const string Property = nameof(Property);
        public const string Context = nameof(Context);
        public const string Name = nameof(Name);
        public const string Value = nameof(Value);
        public const string Update = nameof(Update);
        public const string EditableKey = nameof(EditableKey);
        public const string DisableEdit = nameof(DisableEdit);
        public const string NavigationPath = nameof(NavigationPath);
        public const string PropertyRetrieved = nameof(PropertyRetrieved);
        public const string PropertiesRetrieved = nameof(PropertiesRetrieved);
        public const string AllowDelete = nameof(AllowDelete);
        public const string None = nameof(None);
        public static class Color
        {
            private static readonly List<string> s_pastels = new()
            {
                "#BC85A3",
                "#9E6B55",
                "#2E8364",
                "#A02C2D",
                "#323232",
            };
            public static IEnumerable<string> GetPastels()
            {
                for (var i = 0; i < 1000; i++)
                    yield return s_pastels[i % s_pastels.Count];
            }
        }
    }
}
