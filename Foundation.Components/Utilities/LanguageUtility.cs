using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Utilities
{
    public static class LanguageUtility
    {

        private static readonly CultureInfo[] _supportedCulture = new[] { new CultureInfo("en-CA"), new CultureInfo("fr-CA") };

        public static string GetCurrentApplicationLanguage()
        {
            return CultureInfo.CurrentCulture.Name.Split('-')[0];
        }

        public static CultureInfo[] GetSupportedCulture()
        {
            return _supportedCulture;
        }

        public static string GetOppositeLangauge()
        {
            return (CultureInfo.CurrentCulture.Name == _supportedCulture[0].Name ? _supportedCulture[1] : _supportedCulture[0]).Name.Split('-')[0];
        }

        public static bool IsCultureSupported(string cultureName)
        {
            return _supportedCulture.Contains(new CultureInfo($"{cultureName}-CA"));
        }
    }
}
