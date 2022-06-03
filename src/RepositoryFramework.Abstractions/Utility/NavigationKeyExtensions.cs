using System.Linq.Expressions;
using System.Reflection;

namespace RepositoryFramework
{
    public static class NavigationKeyExtensions
    {
        public static PropertyInfo GetPropertyBasedOnKey<T, TKey>(this Expression<Func<T, TKey>> navigationKey)
            where TKey : notnull
        {
            var keyName = navigationKey.ToString().Split('.').Last();
            Type type = typeof(T);
            return type.GetProperties().First(x => x.Name == keyName);
        }
    }
}
