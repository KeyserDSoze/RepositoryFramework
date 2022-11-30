namespace RepositoryFramework.Web.Components
{
    internal static class Constant
    {
        public const string Key = "Key";
        public const string Entity = "Entity";
        public const string Entities = "Entities";
        public const string ColorEnumerator = "ColorEnumerator";
        public const string Property = "Property";
        public const string Context = "Context";
        public const string Name = "Name";
        public const string Value = "Value";
        public const string Update = "Update";
        public const string EditableKey = "EditableKey";
        public const string DisableEdit = "DisableEdit";
        public const string NavigationPath = "NavigationPath";
        public const string PropertyRetrieved = "PropertyRetrieved";
        public const string PropertiesRetrieved = "PropertiesRetrieved";
        public const string AllowDelete = "AllowDelete";
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
