using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Common.Utilities
{
    /// <summary>
    /// Provides utility methods for handling application language and culture settings.
    /// Supports English (Canada) and French (Canada) cultures.
    /// </summary>
    public static class LanguageUtility
    {
        private static readonly CultureInfo[] _supportedCulture = new[]
        {
            new CultureInfo("en-CA"),
            new CultureInfo("fr-CA")
        };

        /// <summary>
        /// Gets the current application language code ("en" or "fr") based on the current culture.
        /// </summary>
        /// <returns>A two-letter ISO language code ("en" or "fr").</returns>
        public static string GetCurrentApplicationLanguage()
        {
            return CultureInfo.CurrentCulture.Name.Split('-')[0];
        }

        /// <summary>
        /// Gets the list of supported cultures for the application (English Canada and French Canada).
        /// </summary>
        /// <returns>An array of supported <see cref="CultureInfo"/> objects.</returns>
        public static CultureInfo[] GetSupportedCulture()
        {
            return _supportedCulture;
        }

        /// <summary>
        /// Gets the opposite language code ("en" if current is "fr", "fr" if current is "en").
        /// </summary>
        /// <returns>The two-letter ISO code of the opposite language ("en" or "fr").</returns>
        public static string GetOppositeLangauge()
        {
            return (CultureInfo.CurrentCulture.Name == _supportedCulture[0].Name ? _supportedCulture[1] : _supportedCulture[0]).Name.Split('-')[0];
        }

        /// <summary>
        /// Determines whether a given culture name is supported by the application.
        /// </summary>
        /// <param name="cultureName">The two-letter ISO language code (e.g., "en" or "fr").</param>
        /// <returns><c>true</c> if the culture is supported; otherwise, <c>false</c>.</returns>
        public static bool IsCultureSupported(string cultureName)
        {
            return _supportedCulture.Contains(new CultureInfo($"{cultureName}-CA"));
        }

        /// <summary>
        /// Determines if the current application language is English ("en").
        /// </summary>
        /// <returns><c>true</c> if the current language is English; otherwise, <c>false</c>.</returns>
        public static bool IsEnglish()
        {
            return GetCurrentApplicationLanguage() == "en";
        }

        /// <summary>
        /// Determines if the current application language is French ("fr").
        /// </summary>
        /// <returns><c>true</c> if the current language is French; otherwise, <c>false</c>.</returns>
        public static bool IsFrench()
        {
            return GetCurrentApplicationLanguage() == "fr";
        }
    }
}
