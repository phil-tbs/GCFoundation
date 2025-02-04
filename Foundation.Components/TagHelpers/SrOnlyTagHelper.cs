using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-sr-only")]
    public class SrOnlyTagHelper : BaseTagHelper
    {
        public SrOnlyTagEnum Tag { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "tag", Tag);
            base.Process(context, output);
        }
    }
}
