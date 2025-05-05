using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-session-modal")]
    public class FDCPSessionModalTagHelper: FDCPModalTagHelper
    {
        public int SessionTimeout { get; set; }

        public int ReminderTime { get; set; }

        public Uri RefreshURL { get; set; } = default!;

        public Uri LogoutURL { get; set; } = default!;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.Attributes.SetAttribute("data-session-timeout", SessionTimeout);
            output.Attributes.SetAttribute("data-reminder-time", ReminderTime);
            output.Attributes.SetAttribute("data-refresh", RefreshURL);
            output.Attributes.SetAttribute("data-logout", LogoutURL);


            return base.ProcessAsync(context, output);
        }
    }
}
