using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-details")]
    public class DetailsTagHelper : BaseTagHelper
    {
        public required string DetailsTitle { get; set; }

        public bool Open { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "details-title", DetailsTitle);
            AddAttributeIfNotNull(output, "open", Open);
            base.Process(context, output);
        }
    }
}
