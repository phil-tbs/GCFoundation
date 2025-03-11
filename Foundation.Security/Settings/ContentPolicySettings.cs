using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Security.Settings
{
    public class ContentPolicySettings
    {
        public List<string> JavascriptCDN { get; set; } = new List<string>();

        public List<string> CssCDN { get; set; } = new List<string>();

        public List<string> CssCDNHash { get; set; } = new List<string>();

        public List<string> FontCDN { get; set; } = new List<string>();
    }
}
