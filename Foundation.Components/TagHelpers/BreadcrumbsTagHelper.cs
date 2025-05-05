using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-breadcrumbs")]
    public class BreadcrumbsTagHelper : BaseTagHelper
    {
        public bool HideCanadaLink { get; set; }
        public Language Language { get; set; } = Language.en;
        public BreadcrumbsTagHelper() { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "hide-canada-link", HideCanadaLink);
            AddAttributeIfNotNull(output, "lang", Language);

            base.Process(context, output);
        }
    }
}
