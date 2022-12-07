namespace RepositoryFramework.Web
{
    internal sealed class PropertyUiMapper<T, TKey> : IPropertyUiMapper<T, TKey>
        where TKey : notnull
    {
        private readonly IPropertyUiHelper<T, TKey> _propertyUiHelper;

        public PropertyUiMapper(IPropertyUiHelper<T, TKey> propertyUiHelper, IUiMapper<T, TKey> uiMapper)
        {
            _propertyUiHelper = propertyUiHelper;
            uiMapper.Map(_propertyUiHelper);
        }
        public Task<Dictionary<string, PropertyUiSettings>> ValuesAsync(IServiceProvider serviceProvider)
            => _propertyUiHelper.SettingsAsync(serviceProvider);
    }
}
