using RepositoryFramework;
using RepositoryFramework.Web;
using RepositoryFramework.Web.Components;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryBuilderExtensions
    {
        public static IRepositoryBuilder<T, TKey> SetDefaultUiRoot<T, TKey>(
            this IRepositoryBuilder<T, TKey> builder)
            where TKey : notnull
        {
            AppInternalSettings.Instance.RootName = typeof(T).Name;
            return builder;
        }
        public static IRepositoryBuilder<T, TKey> DoNotExposeInUi<T, TKey>(
            this IRepositoryBuilder<T, TKey> builder)
            where TKey : notnull
        {
            AppInternalSettings.Instance.NotExposableRepositories.Add(typeof(T).Name);
            return builder;
        }
        public static IRepositoryBuilder<T, TKey> MapPropertiesForUi<T, TKey, TUiMapper>(
            this IRepositoryBuilder<T, TKey> builder)
            where TKey : notnull
            where TUiMapper : class, IUiMapper<T, TKey>
        {
            builder.Services.AddSingleton<IPropertyUiHelper<T, TKey>, PropertyUiHelper<T, TKey>>();
            builder.Services.AddSingleton<IUiMapper<T, TKey>, TUiMapper>();
            builder.Services.AddSingleton<IPropertyUiMapper<T, TKey>, PropertyUiMapper<T, TKey>>();
            return builder;
        }
    }
}
