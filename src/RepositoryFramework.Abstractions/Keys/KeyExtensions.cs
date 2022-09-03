using System.Reflection;
using System.Text;

namespace RepositoryFramework
{
    public static class KeyExtensions
    {
        public static string AsString<TKey>(this TKey key)
            where TKey : notnull
            => Stringify(key, false, string.Empty);
        public static string AsStringWithName<TKey>(this TKey key)
            where TKey : notnull
            => Stringify(key, true, string.Empty);
        public static string AsStringWithPrefix<TKey>(this TKey key, string prefix)
            where TKey : notnull
            => Stringify(key, false, prefix);
        public static string AsStringWithNameAndPrefix<TKey>(this TKey key, string prefix)
            where TKey : notnull
            => Stringify(key, true, prefix);
        private const char DefaultSeparator = '_';
        private static string Stringify<TStringifiable>(TStringifiable item, bool withName, string prefix)
        {
            if (item == null)
                return string.Empty;
            var type = item.GetType();
            StringBuilder builder = new();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                builder.Append(prefix);
                builder.Append(DefaultSeparator);
            }
            if (withName)
            {
                builder.Append(type.Name);
                builder.Append(DefaultSeparator);
            }
            if (item.IsPrimitive())
                builder.Append(item);
            else
                builder.Append(string.Join(DefaultSeparator,
                    type.FetchProperties().Select(property => Stringify(property.GetValue(item), false, string.Empty))));
            return builder.ToString();
        }
    }
}
