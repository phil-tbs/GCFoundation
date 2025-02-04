using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-side-nav")]
    public class SideNavigationTagHelper : BaseTagHelper
    {
        public required string Label { get; set; }
        public LanguageEnum Lang { get; set; } = LanguageEnum.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "lang", Lang);
            base.Process(context, output);
        }
    }
}
