using Foundation.Components.Services.Interfaces;
using Microsoft.Extensions.Localization;

namespace Foundation.Components.Services
{
    /// <summary>
    /// A service that provides localized values for breadcrumb keys.
    /// This service uses <see cref="IStringLocalizer{T}"/> to retrieve localized strings for breadcrumbs.
    /// </summary>
    /// <typeparam name="T">The type used to retrieve localization resources.</typeparam>
    public class BreadcrumbsLocalizationService<T> : IBreadcrumbsLocalizationService
    {
        private readonly IStringLocalizer<T> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreadcrumbsLocalizationService{T}"/> class.
        /// </summary>
        /// <param name="localizer">An instance of <see cref="IStringLocalizer{T}"/> used for retrieving localized strings.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="localizer"/> is null.</exception>
        public BreadcrumbsLocalizationService(IStringLocalizer<T> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Retrieves the localized value for the given breadcrumb key.
        /// </summary>
        /// <param name="key">The key used to retrieve the localized value.</param>
        /// <returns>The localized value associated with the specified key.</returns>
        public string GetLocalizeValue(string key)
        {
            var localizedString = _localizer[key];
            return localizedString.Value;
        }
    }
}
