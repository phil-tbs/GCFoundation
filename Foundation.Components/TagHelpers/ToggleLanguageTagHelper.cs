using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-lang-toggle")]
    public class ToggleLanguageTagHelper: BaseTagHelper
    {
        public required string Href { get; set; }

        public Language Lang { get; set; } = Language.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "href", Href);
            AddAttributeIfNotNull(output, "lang", Lang);
            base.Process(context, output);
        }
    }
}
