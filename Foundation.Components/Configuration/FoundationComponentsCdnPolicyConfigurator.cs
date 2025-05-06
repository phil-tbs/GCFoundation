using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Common.Settings;
using Microsoft.Extensions.Options;

namespace Foundation.Components.Configuration
{
    /// <summary>
    /// Configures the CDN and security settings for the content security policy.
    /// This ensures all component-related CDN URLs and hashes are injected into the CSP.
    /// </summary>
    public class FoundationComponentsCdnPolicyConfigurator : IConfigureOptions<FoundationContentPolicySettings>
    {
        private readonly FoundationComponentsSettings _componentSettings;

        public FoundationComponentsCdnPolicyConfigurator(IOptions<FoundationComponentsSettings> componentSettings)
        {
            ArgumentNullException.ThrowIfNull(componentSettings, nameof(componentSettings));

            _componentSettings = componentSettings.Value;
        }
        public void Configure(FoundationContentPolicySettings options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            var jsCDNs = Enumerable.Empty<string>();
            var cssCDNs = Enumerable.Empty<string>();
            var cssHashes = Enumerable.Empty<string>();
            var fontCDNs = Enumerable.Empty<string>();

            if (_componentSettings.UsingBootstrapCDN)
            {
                jsCDNs = jsCDNs.Append(_componentSettings.BootstrapJSCDN.Host.ToString());
                cssCDNs = cssCDNs.Append(_componentSettings.BootstrapCSSCDN.Host.ToString());
            }

            jsCDNs = jsCDNs.Append(_componentSettings.GCDSJavaScriptCDN.Host.ToString());
            cssCDNs = cssCDNs
                .Append(_componentSettings.GCDSCssCDN.Host.ToString())
                .Append(_componentSettings.FontAwesomeCDN.Host.ToString());

            cssHashes = FoundationComponentsSettings.GCDSCssCDNHash
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
