using Foundation.Components.Services.Interfaces;
using Microsoft.Extensions.Localization;

namespace Foundation.Components.Services
{
    public class BreadcrumbsLocalizationService<T> : IBreadcrumbsLocalizationService
    {
        private readonly IStringLocalizer<T> _localizer;
        

        public BreadcrumbsLocalizationService(IStringLocalizer<T> localizer){
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }
        public string GetLocalizeValue(string key)
        {
            var localizedString = _localizer[key];
            return localizedString.Value;
        }
    }
}
