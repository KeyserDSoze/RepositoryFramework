using Microsoft.Extensions.Localization;
using RepositoryFramework.Web.Components.Resources;

namespace RepositoryFramework.Web.Components.Business.Language
{
    internal sealed class LocalizationHandler : ILocalizationHandler
    {
        private readonly IStringLocalizer? _sharedLocalizer;
        private readonly Dictionary<string, IStringLocalizer> _localizations = new();
        public LocalizationHandler(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IStringLocalizer<SharedResource>)) is IStringLocalizer defaultLocalizer)
                _sharedLocalizer = defaultLocalizer;
            foreach (var localizationInterface in RepositoryLocalizationOptions.Instance.LocalizationInterfaces)
                if (serviceProvider.GetService(localizationInterface.Value) is IStringLocalizer localizer)
                    _localizations.Add(localizationInterface.Key, localizer);
        }
        public string Get(string value)
        {
            if (_sharedLocalizer != null)
                return _sharedLocalizer[value];
            return value;
        }
        public string Get<T>(string value)
        {
            var name = typeof(T).FullName!;
            if (_localizations.ContainsKey(name))
                return _localizations[name][value];
            return value;
        }
    }
}
