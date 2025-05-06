using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Common.Utilities;

namespace Foundation.Common.Settings
{
    public class FoundationComponentsSettings
    {
        /// <summary>
        /// Gc design system css CDN
        /// </summary>
        public Uri GCDSCssCDN
        {
            get
            {
                return new Uri($"https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@{GCDSVersion}/dist/gcds/gcds.css");
            }
        }

        /// <summary>
        /// Gc Design css hash to execute inline styles
        /// </summary>
        public static string GCDSCssCDNHash
        {
            get
            {
                return "'sha256-wdabfDcuif2zK/ylZhFm5RyLtTWesKFJRNnvzHFPrPs=' 'sha256-LovNkyKf6BdeuYHC6NGHXX9NcrDeLb8ho1xZrkXnC0g='";
            }
        }

        /// <summary>
        /// Gc Design system javascript CDN
        /// </summary>
        public Uri GCDSJavaScriptCDN
        {
            get
            {
                return new Uri($"https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@{GCDSVersion}/dist/gcds/gcds.esm.js");
            }
        }

        /// <summary>
        /// Font awsome CDN
        /// </summary>
        public Uri FontAwesomeCDN
        {
            get
            {
                return new Uri($"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/{FontAwesomeVersion}/css/all.min.css");
            }
        }

        /// <summary>
        /// GC design system version
        /// </summary>
        public string GCDSVersion { get; set; } = "0.34.1";

        /// <summary>
        /// Font awesome version
        /// </summary>
        public string FontAwesomeVersion { get; set; } = "6.4.2";

        /// <summary>
        /// If using bootstrap CDN
        /// </summary>
        public bool UsingBootstrapCDN { get; set; }

        /// <summary>
        /// Version of bootstrap used
        /// </summary>
        public string BootstrapCDNVersion { get; set; } = "5.3.3";

        /// <summary>
        /// Bootstrap CSS CDN
        /// </summary>
        public Uri BootstrapCSSCDN
        {
            get
            {
                return new Uri($"https://cdn.jsdelivr.net/npm/bootstrap@{BootstrapCDNVersion}/dist/css/bootstrap.min.css");
            }
        }

        /// <summary>
        /// Bootstrap javascript CDN
        /// </summary>
        public Uri BootstrapJSCDN
        {
            get
            {
                return new Uri($"https://cdn.jsdelivr.net/npm/bootstrap@{BootstrapCDNVersion}/dist/js/bootstrap.bundle.min.js");
            }
        }

        public string ApplicationNameEn { get; set; } = string.Empty;
        public string ApplicationNameFr { get; set; } = string.Empty;

        /// <summary>
        /// Return the application name depending on the language
        /// </summary>
        public string ApplicationName
        {
            get
            {
                return LanguageUtility.IsEnglish() ? ApplicationNameEn : ApplicationNameFr;
            }
        }

        /// <summary>
        /// The link where you support is in french
        /// You can also add mailto:
        /// </summary>
        public string SupportLinkFr { get; set; } = default!;

        /// <summary>
        /// The link where you support is in english
        /// You can also add mailto:
        /// </summary>
        public string SupportLinkEn { get; set; } = default!;
    }
}
