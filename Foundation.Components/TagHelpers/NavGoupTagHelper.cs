using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-nav-group")]
    public class NavGoupTagHelper: BaseTagHelper
    {
        public required string MenuLabel { get; set; }

        public required string OpenTrigger { get; set; }

        public bool Open { get; set; }

        public string? CloseTrigger { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "menu-label", MenuLabel);
            AddAttributeIfNotNull(output, "open-trigger", OpenTrigger);
            AddAttributeIfNotNull(output, "open", Open);
            AddAttributeIfNotNull(output, "close-trigger", CloseTrigger);
            base.Process(context, output);
        }

    }
}
