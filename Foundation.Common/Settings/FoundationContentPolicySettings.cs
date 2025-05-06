using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Common.Settings
{
    /// <summary>
    /// Represents the configuration settings used to build a Content Security Policy (CSP).
    /// These settings define which external resources (CDNs, fonts, inline hashes) are allowed by the application.
    /// </summary>
    public class FoundationContentPolicySettings
    {
        /// <summary>
        /// Gets or sets the list of JavaScript CDN hosts that are allowed to load scripts.
        /// These will be added to the 'script-src' directive in the CSP header.
        /// Example: "https://cdn.jsdelivr.net"
        /// </summary>
        public IEnumerable<string> JavascriptCDN { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Gets or sets the list of CSS CDN hosts that are allowed to load stylesheets.
        /// These will be added to the 'style-src' directive in the CSP header.
        /// Example: "https://fonts.googleapis.com"
        /// </summary>
        public IEnumerable<string> CssCDN { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Gets or sets the list of SHA-256 hashes that allow specific inline styles.
        /// These hashes will also be added to the 'style-src' directive to support safe inline styles.
        /// Example: "'sha256-AbCdEf123=='"
        /// </summary>
        public IEnumerable<string> CssCDNHash { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Gets or sets the list of font CDN hosts that are allowed to load fonts.
        /// These will be added to the 'font-src' directive in the CSP header.
        /// Example: "https://fonts.gstatic.com"
        /// </summary>
        public IEnumerable<string> FontCDN { get; set; } = Enumerable.Empty<string>();
    }
}
