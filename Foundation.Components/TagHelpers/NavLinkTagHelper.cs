using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-nav-link")]
    public class NavLinkTagHelper: BaseTagHelper
    {
        public required string Href { get; set; }

        public bool? Current { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "href", Href);
            AddAttributeIfNotNull(output, "current", Current);
            base.Process(context, output);
        }

    }
}
