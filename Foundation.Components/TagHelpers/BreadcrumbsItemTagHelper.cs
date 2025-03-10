using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-breadcrumbs-item")]
    public class BreadcrumbsItemTagHelper : BaseTagHelper
    {
        public string? Href { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "href", Href);

            base.Process(context, output);
        }
    }
}
