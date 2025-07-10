using GCFoundation.Common.Settings;
using Microsoft.Extensions.Options;

namespace GCFoundation.Components.Configuration
{
    /// <summary>
    /// Configures the CDN and security settings for the content security policy.
    /// This ensures all component-related CDN URLs and hashes are injected into the CSP.
    /// </summary>
    public class FoundationComponentsCdnPolicyConfigurator : IConfigureOptions<GCFoundationContentPolicySettings>
    {
        private readonly GCFoundationComponentsSettings _componentSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoundationComponentsCdnPolicyConfigurator"/> class.
        /// </summary>
        /// <param name="componentSettings">The options containing the foundation components settings.</param>
        /// <exception cref="ArgumentNullException">Thrown when the componentSettings parameter is null.</exception>
        public FoundationComponentsCdnPolicyConfigurator(IOptions<GCFoundationComponentsSettings> componentSettings)
        {
            ArgumentNullException.ThrowIfNull(componentSettings, nameof(componentSettings));

            _componentSettings = componentSettings.Value;
        }

        /// <summary>
        /// Configures the content security policy settings for the application, including the allowed CDNs and hashes.
        /// </summary>
        /// <param name="options">The content policy settings to configure.</param>
        /// <exception cref="ArgumentNullException">Thrown when the options parameter is null.</exception>
        public void Configure(GCFoundationContentPolicySettings options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            var jsCDNs = Enumerable.Empty<string>();
            var cssCDNs = Enumerable.Empty<string>();
            var cssHashes = Enumerable.Empty<string>();
            var fontCDNs = Enumerable.Empty<string>();


            jsCDNs = jsCDNs.Append(_componentSettings.GCDSJavaScriptCDN.Host.ToString());
            cssCDNs = cssCDNs
                .Append(_componentSettings.GCDSCssCDN.Host.ToString())
                .Append(_componentSettings.FontAwesomeCDN.Host.ToString());

            cssHashes = GCFoundationComponentsSettings.GCDSCssCDNHash
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            fontCDNs = fontCDNs
                .Append(_componentSettings.FontAwesomeCDN.Host.ToString())
                .Append(_componentSettings.GCDSCssCDN.Host.ToString());

            options.JavascriptCDN = jsCDNs;
            options.CssCDN = cssCDNs;
            options.CssCDNHash = cssHashes;
            options.FontCDN = fontCDNs;
        }
    }
}
