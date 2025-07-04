using GCFoundation.Common.Utilities;

namespace GCFoundation.Common.Settings
{
    /// <summary>
    /// Represents configuration settings for frontend dependencies and application metadata.
    /// Provides centralized access to CDN URIs for GC Design System, Font Awesome, Bootstrap,
    /// and multilingual application information.
    /// </summary>
    public class GCFoundationComponentsSettings
    {
        /// <summary>
        /// Gets the URI for the GC Design System CSS from the CDN.
        /// </summary>
        public Uri GCDSCssCDN
        {
            get
            {
                return new Uri($"https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@{GCDSVersion}/dist/gcds/gcds.css");
            }
        }

        /// <summary>
        /// Gets the SHA-256 hash used for inline GC Design System CSS styles for CSP (Content Security Policy).
        /// </summary>
        public static string GCDSCssCDNHash
        {
            get
            {
                return "'sha256-wdabfDcuif2zK/ylZhFm5RyLtTWesKFJRNnvzHFPrPs=' 'sha256-LovNkyKf6BdeuYHC6NGHXX9NcrDeLb8ho1xZrkXnC0g=' 'sha256-h0iBPaGm9POpej1E2Xyl3CW6D/1Nw7OWarnofqER01I=' 'sha256-biLFinpqYMtWHmXfkA1BPeCY0/fNt46SAZ+BBk5YUog='";
            }
        }

        /// <summary>
        /// Gets the URI for the GC Design System JavaScript module from the CDN.
        /// </summary>
        public Uri GCDSJavaScriptCDN
        {
            get
            {
                return new Uri($"https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@{GCDSVersion}/dist/gcds/gcds.esm.js");
            }
        }

        /// <summary>
        /// Gets the URI for the Font Awesome CSS from the CDN.
        /// </summary>
        public Uri FontAwesomeCDN
        {
            get
            {
                return new Uri($"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/{FontAwesomeVersion}/css/all.min.css");
            }
        }

        /// <summary>
        /// Gets or sets the version of the GC Design System being used.
        /// </summary>
        public string GCDSVersion { get; set; } = "0.35.0";

        /// <summary>
        /// Gets or sets the version of Font Awesome being used.
        /// </summary>
        public string FontAwesomeVersion { get; set; } = "6.4.2";

        /// <summary>
        /// Gets or sets whether Bootstrap is used via CDN.
        /// </summary>
        public bool UsingBootstrapCDN { get; set; }

        /// <summary>
        /// Gets or sets the version of Bootstrap being used.
        /// </summary>
        public string BootstrapCDNVersion { get; set; } = "5.3.3";

        /// <summary>
        /// Gets the URI for the Bootstrap CSS from the CDN.
        /// </summary>
        public Uri BootstrapCSSCDN
        {
            get
            {
                return new Uri($"https://cdn.jsdelivr.net/npm/bootstrap@{BootstrapCDNVersion}/dist/css/bootstrap.min.css");
            }
        }

        /// <summary>
        /// Gets the URI for the Bootstrap JavaScript bundle from the CDN.
        /// </summary>
        public Uri BootstrapJSCDN
        {
            get
            {
                return new Uri($"https://cdn.jsdelivr.net/npm/bootstrap@{BootstrapCDNVersion}/dist/js/bootstrap.bundle.min.js");
            }
        }

        /// <summary>
        /// Gets or sets the English name of the application.
        /// </summary>
        public string ApplicationNameEn { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the French name of the application.
        /// </summary>
        public string ApplicationNameFr { get; set; } = string.Empty;

        /// <summary>
        /// Gets the application name based on the current language context.
        /// </summary>
        public string ApplicationName
        {
            get
            {
                return LanguageUtility.IsEnglish() ? ApplicationNameEn : ApplicationNameFr;
            }
        }

        /// <summary>
        /// Gets or sets the support link (or mailto) for French users.
        /// </summary>
        public string SupportLinkFr { get; set; } = default!;

        /// <summary>
        /// Gets or sets the support link (or mailto) for English users.
        /// </summary>
        public string SupportLinkEn { get; set; } = default!;
    }
}
