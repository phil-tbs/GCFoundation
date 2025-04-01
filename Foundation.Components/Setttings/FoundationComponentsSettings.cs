using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Setttings
{
    public class FoundationComponentsSettings
    {
        public Uri GCDSCssCDN { 
            get
            {
                return new Uri($"https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@{GCDSVersion}/dist/gcds/gcds.css");
            }
        }

        public string GCDSCssCDNHash
        {
            get
            {
                return "'sha256-wdabfDcuif2zK/ylZhFm5RyLtTWesKFJRNnvzHFPrPs='";
            }
        }

        public Uri GCDSJavaScriptCDN { 
            get
            {
                return new Uri($"https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@{GCDSVersion}/dist/gcds/gcds.esm.js");
            }
        }

        public Uri FontAwesomeCDN
        {
            get
            {
                return new Uri($"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/{FontAwesomeVersion}/css/all.min.css");
            }
        }

        public string GCDSVersion { get; set; } = "0.30.0";

        public string FontAwesomeVersion { get; set; } = "6.4.2";

        public bool UsingBootstrapCDN { get; set; } = false;

        public string BootstrapCDNVersion { get; set; } = "5.3.3";

        public Uri BootstrapCSSCDN
        {
            get
            {
                return new Uri($"https://cdn.jsdelivr.net/npm/bootstrap@{BootstrapCDNVersion}/dist/css/bootstrap.min.css");
            }
        }

        public Uri BootstrapJSCDN
        {
            get
            {
                return new Uri($"https://cdn.jsdelivr.net/npm/bootstrap@{BootstrapCDNVersion}/dist/js/bootstrap.bundle.min.js");
            }
        }
    }
}
