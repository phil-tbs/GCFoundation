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

            // Ensure Bootstrap CDN is added if it's used
            if (_componentSettings.UsingBootstrapCDN)
            {
                options.JavascriptCDN.Add(_componentSettings.BootstrapJSCDN.Host.ToString());
                options.CssCDN.Add(_componentSettings.BootstrapCSSCDN.Host.ToString());
            }

            // Add GC Design System CDNs
            options.JavascriptCDN.Add(_componentSettings.GCDSJavaScriptCDN.Host.ToString());
            options.CssCDN.Add(_componentSettings.GCDSCssCDN.Host.ToString());
            options.CssCDNHash.AddRange(_componentSettings.GCDSCssCDNHash.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            options.CssCDN.Add(_componentSettings.FontAwesomeCDN.Host.ToString());


            // Add FontAwesome CDN
            options.FontCDN.Add(_componentSettings.FontAwesomeCDN.Host.ToString());
            options.FontCDN.Add(_componentSettings.GCDSCssCDN.Host.ToString());
        }
    }
}
